using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    public class InformationPanel
    {
        private Form1 MainForm;
        public List<string> Tips { get; }
        public InformationPanel(Form1 mainForm)
        {
            MainForm = mainForm;
            Tips = new()
            {
                "Downscaling reduces image dimensions and pixel count, shrinking file size.",
                "It makes sharing or storing images online easier, needing less bandwidth and space.",
                "Excessive downscaling, however, can degrade detail and clarity, lowering quality.",
                "Effective downscaling uses algorithms to minimize loss, preserving visual integrity.",
            };
        }
        public void ShowMessage(string message, MessageType type)
        {
            MainForm.InfoPnl.Visible = true;
            MainForm.InfoLbl2.Text = message;
            switch (type)
            {
                case MessageType.Info:
                    MainForm.InfoPnl.BackColor = Color.Orange;
                    break;
                case MessageType.Error:
                    MainForm.InfoPnl.BackColor = Color.Tomato;
                    break;
                case MessageType.Success:
                    MainForm.InfoPnl.BackColor = Color.MediumSeaGreen;
                    break;
                default:
                    break;
            }
        }     
        public void ShowTip()
        {
            string currentInfo = MainForm.InfoLbl2.Text;
            string randomTip;
            while (true)
            {
                Random r = new();
                randomTip = Tips[r.Next(Tips.Count)];

                if (currentInfo != randomTip) break;
            }
            
            ShowMessage(randomTip, MessageType.Info);
        }
        public void ShowResizedSuccessfully(string imgName, double percentage)
        {
            ShowMessage($"Image '{imgName}' is resized succesfully to {percentage}% and the performance is logged.", MessageType.Success);
        }
        public void ShowSavedSuccessfully(string imgName)
        {
            ShowMessage($"Downsized image '{imgName}' is saved succesfully", MessageType.Success);
        }
        public void ShowInputWarning()
        {
            ShowMessage("Please make sure that an image is selected and a percentage is entered between 10 and 99", MessageType.Error);
        }
        public void Clear()
        {
            MainForm.InfoPnl.Visible = false;
            MainForm.InfoLbl2.Text = string.Empty;
        }
        public enum MessageType
        {
            Info,
            Error,
            Success
        }
    }
}
