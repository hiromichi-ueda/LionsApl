using Xamarin.Forms;

namespace LionsApl.Content
{
    internal class ContentUtil
    {
        private readonly string OffVal = "0";
        private readonly string OnStr = "有り";
        private readonly string OffStr = "無し";

        public string LabelOnOff(string item)
        {
            string retStr = string.Empty;
            if (item != string.Empty)
            {
                if (item == OffVal)
                {
                    retStr = OffStr;
                }
                else
                {
                    retStr = OnStr;
                }
            }
            return retStr;
        }

        public Label LabelCreate(string labelStr, 
                                 NamedSize fontsize, 
                                 LayoutOptions voption, 
                                 string styleclass,
                                 int rowNum,
                                 int columnNum,
                                 int columnSpan)
        {
            Label label = new Label
            {
                Text = labelStr,
                FontSize = Device.GetNamedSize(fontsize, typeof(Label)),
                VerticalOptions = voption,
                StyleClass = new[] { styleclass }
            };
            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, columnNum);
            Grid.SetColumnSpan(label, columnSpan);
            return label;
        }
    }
}
