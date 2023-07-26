using System;
using System.Globalization;

namespace FEC_Michiten_ClassLibrary.Util
{

    /// <summary>
    /// 時差を考慮した日時を扱うDateTimeOffsetインスタンス操作ヘルパー
    /// </summary>
    public class DateTimeOffsetHelper
    {

        /// <summary>
        /// 日付文字列のフォーマット SONY対応も含む(fff)
        /// </summary>
        private static readonly string[] READ_TIME_FORMAT_LIST = { 
            "ddMMyyHHmmss.ffzzz",
            "ddMMyyHHmmss.fffzzz",
            "yyyy-MM-ddTHH:mm:sszzz"
        };


        private TimeZoneInfo _timeZoneInfo = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="timeZoneInfo"></param>時差処理を行うためのTimeZoneInfo
        public DateTimeOffsetHelper(TimeZoneInfo timeZoneInfo)
        {
            _timeZoneInfo = timeZoneInfo;
        }


        /// <summary>
        /// NMEA文字列から時差を考慮したDateTimeOffsetを生成して返す
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        public DateTimeOffset CreateLocalOffset(string timeStr)
        {
            // NMEA文字列からUTC時刻作成
            var utcDTOffset = DateTimeOffset.ParseExact($"{timeStr}+00:00",
                                                        READ_TIME_FORMAT_LIST,
                                                        DateTimeFormatInfo.InvariantInfo,
                                                        DateTimeStyles.None
                                                        );
            // 時差を考慮したものを生成
            return TimeZoneInfo.ConvertTime(utcDTOffset, _timeZoneInfo);
        }

        /// <summary>
        /// KML内の時刻文字から画面利用のための現地時間DateTimeを生成
        /// </summary>
        /// <param name="kmlTimeStr"></param> 例）2020-08-18T04:01:40+09:00
        /// <returns></returns>
        public static DateTime? CreateLocalTimeFromKmlString(string kmlTimeStr)
        {
            var result = DateTimeOffset.TryParseExact(kmlTimeStr,
                                            READ_TIME_FORMAT_LIST,
                                            DateTimeFormatInfo.InvariantInfo,
                                            DateTimeStyles.None,
                                            out DateTimeOffset dateTimeOffset
                                            );
            // Parse失敗時はnull返す
            if (!result) return null;

            // 画面側に合わせてDateTimeに変換
            return dateTimeOffset.DateTime;
        }

    }

}
