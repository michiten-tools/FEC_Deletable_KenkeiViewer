using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Util
{
    public class CategoryDefine
    {

        public static Dictionary<int, string> CategoryStr = new Dictionary<int, string>
        {
            {1,"（A）道路標識"},
            {2,"（B）道路情報提供装置（添加式含む）"},
            {3,"（C）道路照明施設"},
            {4,"（D）その他"},
            {5,"（K）街路樹"},
            {6,"（L）カーブミラー"},
            {7,"（O）帯状道路施設（起点）"},
            {8,"（P）帯状道路施設（終点）"},
            {9,"（S）解析範囲（起点）"},
            {10,"（T）解析範囲（終点）"},
            {11,"（U）予備1"},
            {12,"（V）予備2"},
            {13,"（W）予備3"},
            {14,"（X）不明"},
            {101, "（A）道路標識（単）"},
            {102, "（B）道路標識（複）"},
            {103, "（C）道路標識（確認用）"},
            {104, "（D）道路標識（予備）"},
            {105, "（E）大型標識"},
            {106, "（F）不明標識"},
            {109, "（O）車線"},
            {110, "（P）道路標示（予備）"},
            {112, "（S）解析範囲（起点）"},
            {113, "（T）解析範囲（終点）"},
            {111, "（L）道路標示"},
            {107, "（M）帯状道路標示（起点）"},
            {108, "（N）帯状道路標示（終点）"},
            {114, "（Q）不明標示"},
            {201,"（A）電路柱（A）"},
            {202,"（B）電路柱（B）"},
            {203,"（C）電路柱（M）"},
            {204,"（D）地上子"},
            {205,"（K）信号機"},
            {206,"（L）踏切"},
            {207,"（O）切替ポイント"},
			{208,"（V）支持金具" }
        };

        public static string GetCategoryChar(int index)
		{
			if (index == 1)
				return "A";
			else if (index == 2)
				return "B";
			else if (index == 3)
				return "C";
			else if (index == 4)
				return "D";

			else if (index == 5)
				return "K";
			else if (index == 6)
				return "L";

			else if (index == 7)
				return "O";
			else if (index == 8)
				return "P";

			else if (index == 9)
				return "S";
			else if (index == 10)
				return "T";

			else if (index == 11)
				return "U";
			else if (index == 12)
				return "V";
			else if (index == 13)
				return "W";

			else if (index == 14)
				return "X";
			else
				return null;
		}

		public static string GetCategoryStr(int index)
		{
			if (index == 1)
				return "（A）道路標識";
			else if (index == 2)
				return "（B）道路情報提供装置（添加式含む）";
			else if (index == 3)
				return "（C）道路照明施設";
			else if (index == 4)
				return "（D）その他";

			else if (index == 5)
				return "（K）街路樹";
			else if (index == 6)
				return "（L）カーブミラー";

			else if (index == 7)
				return "（O）帯状道路施設（起点）";
			else if (index == 8)
				return "（P）帯状道路施設（終点）";

			else if (index == 9)
				return "（S）解析範囲（起点）";
			else if (index == 10)
				return "（T）解析範囲（終点）";

			else if (index == 11)
				return "（U）予備1";
			else if (index == 12)
				return "（V）予備2";
			else if (index == 13)
				return "（W）予備3";

			else if (index == 14)
				return "（X）不明";
			else
				return null;
		}

		public static string GetCategoryStrEng(int index)
		{
			if (index == 1)
				return "(A)road sign";
			else if (index == 2)
				return "(B)bulletin board";
			else if (index == 3)
				return "(C)lighting";
			else if (index == 4)
				return "(D)others";

			else if (index == 5)
				return "(K)tree";
			else if (index == 6)
				return "(L)mirror";

			else if (index == 7)
				return "(O)long item start";
			else if (index == 8)
				return "(P)long item end";

			else if (index == 9)
				return "(S)route start";
			else if (index == 10)
				return "(T)route end";

			else if (index == 11)
				return "(U)reserved 1";
			else if (index == 12)
				return "(V)reserved 2";
			else if (index == 13)
				return "(W)reserved 3";

			else if (index == 14)
				return "(X)unidentified";
			else
				return null;
		}

        public static string GetCategoryStrWithoutMark(int index)
        {
            return CategoryStr[index].Remove(0, 3);
        }

        public static string GetCategoryStrEngWithoutMark(int index)
		{
			if (index == 1)
				return "road sign";
			else if (index == 2)
				return "bulletin board";
			else if (index == 3)
				return "lighting";
			else if (index == 4)
				return "others";

			else if (index == 5)
				return "tree";
			else if (index == 6)
				return "mirror";

			else if (index == 7)
				return "long item start";
			else if (index == 8)
				return "long item end";

			else if (index == 9)
				return "route start";
			else if (index == 10)
				return "route end";

			else if (index == 11)
				return "reserved 1";
			else if (index == 12)
				return "reserved 2";
			else if (index == 13)
				return "reserved 3";

			else if (index == 14)
				return "unidentified";
			else
				return null;
		}

		public static int GetCategoryIndex(string str)
		{
			if (str.Equals("道路標識") ||
				str.Equals("road sign"))
				return 1;
			else if (str.Equals("道路情報提供装置（添加式含む）") ||
				str.Equals("bulletin board"))
				return 2;
			else if (str.Equals("道路照明施設") ||
				str.Equals("lighting"))
				return 3;
			else if (str.Equals("その他") ||
				str.Equals("others"))
				return 4;

			else if (str.Equals("街路樹") ||
				str.Equals("tree"))
				return 5;
			else if (str.Equals("カーブミラー") ||
				str.Equals("mirror"))
				return 6;

			else if (str.Equals("帯状道路施設（起点）") ||
				str.Equals("long item start"))
				return 7;
			else if (str.Equals("帯状道路施設（終点）") ||
				str.Equals("long item end"))
				return 8;

			else if (str.Equals("解析範囲（起点）") ||
				str.Equals("route start"))
				return 9;
			else if (str.Equals("解析範囲（終点）") ||
				str.Equals("route end"))
				return 10;

			else if (str.Equals("予備1") ||
				str.Equals("reserved 1"))
				return 11;
			else if (str.Equals("予備2") ||
				str.Equals("reserved 2"))
				return 12;
			else if (str.Equals("予備3") ||
				str.Equals("reserved 3"))
				return 13;
			else if (str.Equals("不明") ||
				str.Equals("unidentified"))
				return 14;
            else if (str.Equals("道路標識（単）")) return 101;
            else if (str.Equals("道路標識（複）")) return 102;
            else if (str.Equals("道路標識（確認用）")) return 103;
            else if (str.Equals("道路標識（予備）")) return 104;
            else if (str.Equals("大型標識")) return 105;
            else if (str.Equals("不明標識")) return 106;
            else if (str.Equals("車線")) return 109;
            else if (str.Equals("道路標示（予備）")) return 110;
            else if (str.Equals("解析範囲（起点）")) return 112;
            else if (str.Equals("解析範囲（終点）")) return 113;
            else if (str.Equals("道路標示")) return 111;
            else if (str.Equals("帯状道路標示（起点）")) return 107;
            else if (str.Equals("帯状道路標示（終点）")) return 108;
            else if (str.Equals("不明標示")) return 114;
            else if (str.Equals("電路柱（A）")) return 201;
            else if (str.Equals("電路柱（B）")) return 202;
            else if (str.Equals("電路柱（M）")) return 203;
            else if (str.Equals("地上子")) return 204;
            else if (str.Equals("信号機")) return 205;
            else if (str.Equals("踏切")) return 206;
            else if (str.Equals("切替ポイント")) return 207;
            else if (str.Equals("電路柱（起点）")) return 201;
            else if (str.Equals("電路柱（起点）")) return 201;
            else if (str.Equals("電路柱（終点）")) return 202;
            else if (str.Equals("地上子")) return 204;
            else if (str.Equals("信号機")) return 205;
            else if (str.Equals("踏切")) return 206;
            else if (str.Equals("距離標")) return 207;
            else if (str.Equals("支持金具")) return 208;
            else
                return -1;
		}
	}
}
