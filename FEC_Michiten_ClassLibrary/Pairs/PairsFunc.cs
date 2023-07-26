using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Michiten_ClassLibrary.Pairs
{
	public class PairsFunc
	{
		public delegate void StatusEventHandler(string msg, EventArgs e);
		public event StatusEventHandler StatusEvent;

		public bool CheckPairsCsv(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				UtilFunc.ErrMsg(Define.ErrMsgIncorrectSelection);
				StatusEvent(Define.ErrMsgIncorrectSelection, null);
				return false;
			}

			if (!File.Exists(file))
			{
				UtilFunc.ErrMsg(Define.ErrMsgFileNotFound);
				StatusEvent(Define.ErrMsgFileNotFound, null);
				return false;
			}

            try
            {
				var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
				stream.Close();
			}
            catch
            {
				UtilFunc.ErrMsg(Define.ErrMsgFileOpened);
				StatusEvent(Define.ErrMsgFileOpened, null);
				return false;
			}

			string frames = Path.Combine(Directory.GetParent(file).ToString(), Define.DirFrames);

			if (!Directory.Exists(frames))
			{
				UtilFunc.ErrMsg($"{frames} {Define.ErrMsgDirectoryNotFound}");
				StatusEvent($"{frames} {Define.ErrMsgDirectoryNotFound}", null);
				return false;
			}

			List<string> jpegFiles = Directory.EnumerateFiles(frames, Define.FilterExtJpeg, SearchOption.TopDirectoryOnly).ToList();
			if (jpegFiles.Count == 0)
			{
				UtilFunc.ErrMsg($"{frames} {Define.ErrMsgImgNotExists}");
				StatusEvent($"{frames} {Define.ErrMsgImgNotExists}", null);
				return false;
			}

			return true;
		}

		public List<SignItem> LoadPairsCsv(string file, ref PairsMode mode)
		{
			try
			{
				if (file.Contains("Label"))
					mode = PairsMode.Label;
				else
					mode = PairsMode.Lot;

				List<SignItem> res = new List<SignItem>();
				using (var sr = new StreamReader(file, Define.JIS))
				{
					int count = 0;
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();

						if (line.StartsWith("no"))
						{
							continue;
						}

						string[] strs = line.Split(',');

						if (6 <= strs.Length)
						{
							var newItem = new SignItem
							{
								Index = count,
								CurrentFrame = 0,
								No = strs[0],
								Category = CategoryDefine.GetCategoryIndex(strs[2]),
								TimeTarget = new LatLng
								{
									Lat = double.Parse(strs[4]),
									Lng = double.Parse(strs[3]),
									Time = DateTime.Parse(strs[1]),
								},
								ImageFileName = strs[5],
							};

							// 納品ラベルをチェック
							if (!string.IsNullOrEmpty(newItem.ImageFileName))
							{
								// 納品ラベルにロット番号が含まれているか
								if (newItem.ImageFileName.Contains(newItem.No))
								{
									// 含まれていたら納品ラベルなし→ロット番号
									newItem.Label = newItem.No;
								}
								else
								{
									// 納品ラベル
									newItem.Label = Path.GetFileNameWithoutExtension(newItem.ImageFileName).ToString();

									// 1つでも納品ラベルが入っていたら列表示を変える
									//mode = PairsMode.Label;
								}
							}

							if (strs.Length > 6 && !string.IsNullOrEmpty(strs[6]))
                            {
								// 帯状（終点）のとき
								newItem.LongItem = new LongItemModel
								{
									SrcNoStr = strs[6],
									//DstNo = strs[0]
								};
                            }
							if(strs.Length == 9)
							{
								newItem.address.Ken = strs[7];
                                newItem.address.Sonota = strs[8];
                            }

							// NOTE: 解析範囲は表示対象外（念のため）
							if (newItem.Category == 9 || newItem.Category == 10)
								continue;

							res.Add(newItem);

							count++;
						}
						else
						{
							Debug.WriteLine("> length error");
						}
					}
				}
				return res;
			}
			catch (Exception e)
			{
				Debug.WriteLine($"> {e.ToString()}");
				return null;
			}
		}

		public List<SignItem> FilterCategoryAndWord(List<SignItem> pairs, List<bool> type, string word, PairsMode mode)
		{
			List<SignItem> res = new List<SignItem>();

			foreach (var i in Enumerable.Range(0, type.Count))
			{
				if (!type[i])
					continue;

				// 帯状（終点）のとき、帯状（始点）がfalseだったら追加しない
				if (i == 7 && !type[6])
					continue;

				if (mode.Equals(PairsMode.Lot))
				{
					var syncItem = pairs.Where(x => x.Category == i + 1 && x.No.Contains(word)).ToList();

					if (syncItem != null)
						res.AddRange(syncItem);
				}
				else if (mode.Equals(PairsMode.Label))
                {
					var syncItem = pairs.Where(x => x.Category == i + 1 && x.Label.Contains(word)).ToList();

					if (syncItem != null)
						res.AddRange(syncItem);
				}
			}

			return res;
		}

		/// <summary>
		/// 長物中間ポイント読込
		/// </summary>
		/// <param name="items">施設リスト</param>
		/// <param name="pairsFile">pairsファイル名</param>
		public void LoadLongItemMiddlePoint(List<SignItem> items, string pairsFile)
		{
			//Pairs.CSVと同じフォルダ内にpairsLongItem.CSVがあるかどうか
			FileInfo inf = new FileInfo(pairsFile);
			string longItemfile = Path.Combine(inf.Directory.FullName, "pairsLongItem.csv");

			//もしなければ何もしない
			if(!File.Exists(longItemfile))
			{

				return;
			}

			//ファイルがあれば読み込む
			using (var sr = new StreamReader(longItemfile, Define.JIS))
			{

				//終了まで読み込む
				while (!sr.EndOfStream)
				{
					//一行読み込む
					string line = sr.ReadLine();

					//ヘッダーなら飛ばす
					if (line.StartsWith("signNo"))
					{
						continue;
					}

					//No、緯度、経度、順位の順に読み込む
					string[] strs = line.Split(',');
					string no = strs[0];
					double lon = double.Parse(strs[2]);
					double lat = double.Parse(strs[1]);
					int order = int.Parse(strs[3]);

					//Noに紐づく施設を取得
					SignItem si = items.FirstOrDefault(c => c.No == no);

					//無ければ次に行く
					if (si == null)
					{
						//エラーにしてもいいけど
						continue;
					}

					//中間ポイントに設定
					LatLngMiddlePoint middlePoint = new LatLngMiddlePoint()
					{
						Lat = lat,
						Lng = lon,
						Order = order
					};

					si.LongItemMiddlePoint.Add(middlePoint);

				}
			}

			//中間ポイントに設定してから、順位でソート
			//全部読み込んでからソートしないと出来ないから
			items.ForEach(item =>
			{
				item.LongItemMiddlePoint.Sort((x, y) => x.Order - y.Order);
			});

		}
	}
}
