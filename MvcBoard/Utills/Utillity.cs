namespace MvcBoard.Utills
{
    public class Utillity
    {
        public class Vaildataion
        {

            // TODO JSON key-value 면 더 좋을 것 같은데 , 아니면 객체 자체를 넘겨받아서 타입별로 알아서 처리하고 복사본 반환? (제네릭)

            /// <summary>
            /// 문자열 배열을 입력 받아, 유효성을 검증하고 Trim 처리하여 반환
            /// </summary>
            /// <param name="Strings"></param>
            /// <returns> IsValid, List<string> </returns
            public static (bool, List<string>/*, Message?*/) CheckStrings(List<string> Strings)
            {
                var cnt = 0;
                List<string> NewStrings = new List<string>();

                for (int i = 0; i < Strings.Count; i++)
                {
                    // 문자열 유효성 검증 로직 (지금은 공백 확인만 함)
                    NewStrings.Add(Strings[i].Trim());

                    if (NewStrings[i].Length == 0)
                    {
                        cnt++;
                    }
                }

                return (cnt == 0, NewStrings);
            }
        }


    }
}
