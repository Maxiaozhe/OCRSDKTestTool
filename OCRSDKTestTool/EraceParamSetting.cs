using DocumentSDKInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRSDKTest
{
    public partial class EraceParamSetting : Form
    {
        public EraceParamSetting()
        {
            InitializeComponent();
        }

        private void InitControls()
        {
            //罫線処理のパラメタ変数初期設定
            InitTableEraseParam();
            //ノイズ除去のパラメタ変数初期設定
            InitNoiseEraseParam();

        }
        /// <summary>
        /// 罫線処理のパラメタ変数設定
        /// </summary>
        private void InitTableEraseParam()
        {
            EraserParams env = TableEraser.Env;
            this.numMinLength.Value = env.MinLenght;
            this.numRatio.Value = env.LineRectRatio;
            this.numMinStep.Value = env.MinScanLength;
            this.numMaxSpace.Value = env.MaxDotSpace;
            this.numHStep.Value = env.HighSpeedStep;
            this.numExtraFrameMargin.Value = env.ExtractFrameMargin;
        }

        /// <summary>
        /// ノイズ除去のパラメタ変数設定
        /// </summary>
        private void InitNoiseEraseParam()
        {
            EraseNoiseParams env = OcrSDK.NoiseEnv;
            //DocumentType
            AddEnumList(this.cmbDocType, env.DocumentType);
            AddEnumList(this.cmbLevel, env.Level);
            AddEnumList(this.cmbNoiseType, env.NoiseType);
            AddEnumList(this.cmbFastMode, env.FastMode);
            if (env.IsAutoSize)
            {
                this.chkAutoSize.Checked = true;
                this.numMaxNoiseSize.Enabled = false;
                this.numMaxNoiseSize.ResetText();
            }
            else
            {
                this.chkAutoSize.Checked = false;
                this.numMaxNoiseSize.Enabled = true;
                this.numMaxNoiseSize.Value = env.MaxNoiseSize;
            }



        }

        private void AddEnumList(ComboBox cmbBox, Enum defValue)
        {
            string[] names = Enum.GetNames(defValue.GetType());
            cmbBox.Items.Clear();
            foreach (string name in names)
            {
                cmbBox.Items.Add(name);
                if (name.Equals(defValue.ToString()))
                {
                    cmbBox.SelectedItem = name;
                }
            }
        }

        private void AddEnumCheckList(Control Parent, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            Parent.Controls.Clear();
            foreach (string name in names)
            {
                CheckBox chkFmt = new CheckBox();
                chkFmt.Text = name;
                Parent.Controls.Add(chkFmt);
            }
        }

        private object GetEnumSelected(ComboBox cmbBox, Type enumType)
        {
            if(cmbBox.SelectedIndex==-1) return null;
            string item=(string)cmbBox.SelectedItem;
            return Enum.Parse(enumType, item);
        }


        private void EraceParamSetting_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //罫線処理のパラメタ変数保存
            SetTableEraserParam();
            //ノイズ除去のパラメタ変数保存
            SetNoiseEraseParam();
            this.Close();
        }

  
        private void SetTableEraserParam()
        {
            EraserParams env = new EraserParams();
            env.MinLenght = (int)this.numMinLength.Value;
            env.LineRectRatio = (int)this.numRatio.Value;
            env.MinScanLength = (int)this.numMinStep.Value;
            env.MaxDotSpace = (int)this.numMaxSpace.Value;
            env.HighSpeedStep = (int)this.numHStep.Value;
            env.ExtractFrameMargin = (int)this.numExtraFrameMargin.Value;
            TableEraser.SetParams(env);
        }

        private void SetNoiseEraseParam()
        {
            EraseNoiseParams env = new EraseNoiseParams();
            DocumentSDKDocumentType docType = (DocumentSDKDocumentType)GetEnumSelected(cmbDocType, typeof(DocumentSDKDocumentType));
            env.DocumentType = docType;
            DocumentSDKLevel level = (DocumentSDKLevel)GetEnumSelected(cmbLevel, typeof(DocumentSDKLevel));
            env.Level = level;
            DocumentSDKFastMode faseMode = (DocumentSDKFastMode)GetEnumSelected(cmbFastMode, typeof(DocumentSDKFastMode));
            env.FastMode = faseMode;
            DocumentSDKNoiseType noiseType = (DocumentSDKNoiseType)GetEnumSelected(cmbNoiseType, typeof(DocumentSDKNoiseType));
            env.NoiseType = noiseType;
            if (this.chkAutoSize.Checked)
            {
                env.MaxNoiseSize = DocumentSDKDefinition.ENSH_MDS_AUTO;
            }
            else
            {
                env.MaxNoiseSize =(short) numMaxNoiseSize.Value;
            }
            OcrSDK.SetParams(env);
        }


        private void chkAutoSize_CheckedChanged(object sender, EventArgs e)
        {
            this.numMaxNoiseSize.Enabled = !chkAutoSize.Checked;
        }


    }
}
