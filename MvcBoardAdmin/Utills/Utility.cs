namespace MvcBoardAdmin.Utills
{
    public class Utility
    {
        /// <summary>
        /// 페이지 인디케이터의 범위를 계산하여 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public static IndicatorRange GetIndicatorRange(IndicatorRangeParams _params)
        {
            // TODO 가능하면 간소화
            int page = _params.Page < 1 ? 1 : _params.Page;
            int count = _params.DisplayPageCount < 1 ? 5 : _params.DisplayPageCount;
            int pageCount = _params.PageCount < 1 ? 1 : _params.PageCount;

            int first_center_index = count / 2 + 1;
            int start = Math.Max(page - (count - 1) / 2, 1);
            int offset_left = (first_center_index > page) ? first_center_index - page : 0;
            int end = Math.Min(page + offset_left + (count - 1) / 2, pageCount);
            int offset_right = (start + count - 1) - end;

            start = Math.Max(start - offset_right, 1);

            return new IndicatorRange { start = start, end = end};
        }

        public class IndicatorRangeParams
        {
            public int DisplayPageCount { get; set; } = 5; // 인디케이터에 노출되는 페이지 수 (홀수)
            public int PageCount { get; set; } = 1; // 총 페이지 수
            public int Page { get; set; } = 1; // 현재 페이지 인덱스
        }

        public class IndicatorRange
        {
            public int start { get; set; }
            public int end { get; set; }
        }
    }

    

}
