using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRSDKTest
{
    class CanvasPicture2:PictureBox
    {
        private ContextMenuStrip deleteMenuStrip;
        private ToolStripMenuItem mnuDelete;
        private System.ComponentModel.IContainer components;
        private float realZoom = 1.0F;

        public event MouseEventHandler MouseSelectBegin;
        public event MouseEventHandler MouseSelectEnd;
        public event MouseEventHandler MouseSelectDrag;
        public event EventHandler<FrameEventArgs> CreateBarcode;
        public event EventHandler<FrameEventArgs> SelectedBarcode;
   

        public float Zoom
        {
            get
            {
                return this.realZoom * 100;
            }
            set
            {
                this.realZoom = value / 100.0F;
                this.SizeMode = PictureBoxSizeMode.Zoom;
                if (this.Image != null)
                {
                    this.Size = new Size((int)(this.Image.Width * realZoom), (int)(this.Image.Height * realZoom));
                }
            }
        }


        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FrameCollection<BarcodeFrame> Frames
        {
            get;
            private set;
        }

        public List<Rectangle> SelectedFrames { get; set; }


        [System.ComponentModel.Browsable(false)]
        public List<TransArea> TranAreas { get; set; }

        public Frame Selection { get; set; }

        public CanvasPicture2()
        {
            this.Frames = new FrameCollection<BarcodeFrame>();
            this.Zoom = 100;
            InitializeComponent();
        }

        public bool AllowAddFrame
        {
            get;
            set;
        }

        public bool ShowFrames
        {
            get;
            set;
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.deleteMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // deleteMenuStrip
            // 
            this.deleteMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDelete});
            this.deleteMenuStrip.Name = "contextMenuStrip1";
            this.deleteMenuStrip.Size = new System.Drawing.Size(99, 26);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(98, 22);
            this.mnuDelete.Text = "削除";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // CanvasPicture2
            // 
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasPicture_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseUp);
            this.deleteMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }


        private void CanvasPicture_Paint(object sender, PaintEventArgs e)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                return;
            }
            ZoomedGraphics zg = new ZoomedGraphics(e.Graphics, realZoom);
            if (this.ShowFrames && this.Frames != null && this.Frames.Items.Count > 0)
            {
                foreach(BarcodeFrame frame in this.Frames.Items)
                {
                    frame.Draw(zg);
                }
            }


        }
        private void DrawTranArea(ZoomedGraphics zg)
        {
            foreach (var area in this.TranAreas)
            {
                Pen pen = Pens.Green;
                switch ((FormSDKInterface.FormSDKStatus)area.status)
                {
                    case FormSDKInterface.FormSDKStatus.FORM_ST_CHAR_GIVEUP:
                        pen = Pens.OrangeRed;
                        break;
                    case FormSDKInterface.FormSDKStatus.FORM_ST_OK:
                        pen = Pens.Green;
                        break;
                    case FormSDKInterface.FormSDKStatus.FORM_ST_OUTOF_RANGE:
                        pen = Pens.HotPink;
                        break;
                    case FormSDKInterface.FormSDKStatus.FORM_ST_TRANS_GIVEUP:
                        pen = Pens.Gray;
                        break;
                    default:
                        break;
                }

                zg.DrawRectangle(pen, area.TransRect);

                pen = Pens.Purple;
                zg.DrawRectangle(pen, area.CharRect);
            }
        }

        public void ShowToolTip(ToolTip tooltip, BarcodeFrame frame)
        {
            PointF zoomPoint = this.ZoomedPoint(frame.StartPoint);
            if (frame.BarcodeFormat != 0)
            {
                tooltip.Show(frame.BarcodeFormat.ToString() + "：" + frame.RawText, this, Point.Round(zoomPoint), 1000);
            }
            else
            {
                tooltip.Show("認識できません。", this, Point.Round(zoomPoint), 1000);
            }
        }

        private Rectangle UnzoomRect(RectangleF rectf)
        {
            return new Rectangle(
                (int)(rectf.X / this.realZoom), 
                (int)(rectf.Y / this.realZoom), 
                (int)(rectf.Width / this.realZoom), 
                (int)(rectf.Height / this.realZoom));
        }
        private Point UnzoomPoint(Point pt)
        {
            return new Point(
                (int)(pt.X / this.realZoom),
                (int)(pt.Y / this.realZoom));
        }

        private RectangleF ZoomedRect(Rectangle rect)
        {
            return new RectangleF(rect.X * this.realZoom, rect.Y * this.realZoom, rect.Width * this.realZoom, rect.Height * realZoom);
        }
        private PointF ZoomedPoint(Point pt)
        {
            return new PointF(pt.X * this.realZoom, pt.Y * this.realZoom);
        }

        private void CanvasPicture_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.AllowAddFrame || !this.ShowFrames)
            {
                return;
            }
            if (this.Selection != null)
            {
                if (!this.Selection.IsTooSmall)
                {
                    DrawXorFrame(this.Selection.Rect);
                }
                this.Selection = null;
            }

            if (this.ShowFrames && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Selection = new Frame(new Point(e.X, e.Y));
                if (this.MouseSelectBegin != null)
                {
                    this.MouseSelectBegin.Invoke(sender, e);
                }
            }
        }



        private void CanvasPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.AllowAddFrame || !this.ShowFrames)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.Selection != null)
            {
                if (!this.Selection.IsTooSmall)
                {
                    DrawXorFrame(this.Selection.Rect);
                }
                this.Selection.EndPoint = new Point(e.X, e.Y);
                if (!this.Selection.IsTooSmall)
                {
                    DrawXorFrame(this.Selection.Rect);
                }
                if (this.MouseSelectDrag != null)
                {
                    this.MouseSelectDrag.Invoke(sender, e);
                }
            }
        }

        private void CanvasPicture_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.AllowAddFrame || !this.ShowFrames)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.Selection != null)
            {
                if (!this.Selection.IsTooSmall)
                {
                    DrawXorFrame(this.Selection.Rect);
                }
                this.Selection.EndPoint = new Point(e.X, e.Y);
                if (!this.Selection.IsTooSmall)
                {
                    Rectangle rect = Selection.Rect;
                    DrawXorFrame(this.Selection.Rect);
                    //Create barcode
                    if (this.CreateBarcode != null)
                    {
                        Rectangle unzoomRect = this.UnzoomRect(rect);
                        BarcodeFrame frame = new BarcodeFrame(unzoomRect)
                        {
                            CanCreate=true
                        };
                        this.Frames.Items.Add(frame);
                        this.CreateBarcode(this, new FrameEventArgs(frame));
                    }
                    DrawXorFrame(this.Selection.Rect);
                    this.Refresh();
                    return;
                }
                else
                {
                    this.Selection = null;
                }
                if (this.MouseSelectEnd != null)
                {
                    this.MouseSelectEnd.Invoke(sender, e);
                }
            }
            Point selPos = this.UnzoomPoint(new Point(e.X, e.Y));
            BarcodeFrame selFrm = this.Frames.GetFrameByPoint(selPos.X, selPos.Y);
            if (selFrm != null && (ModifierKeys & Keys.Control) == Keys.Control && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //Ctrlキー+Left Click（マルチ選択）
                selFrm.IsSelected = true;
                this.Refresh();
            }
            else if (selFrm != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //単一選択
                this.Frames.ClearSelection();
                selFrm.IsSelected = true;
                this.Refresh();
                if (this.SelectedBarcode != null)
                {
                    this.SelectedBarcode(this, new FrameEventArgs(selFrm));
                }
            }
            else if (selFrm == null && this.Frames.ToSelectedRects().Count > 0 && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Frames.ClearSelection();
                this.Refresh();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point clientPos = this.PointToScreen(new Point(e.X, e.Y));
                this.deleteMenuStrip.Show(clientPos);
            }
        }

        private void DrawXorFrame(Rectangle rect)
        {
            using (Graphics g = this.CreateGraphics())
            {
                XorGraphics xg = new XorGraphics(g);

                xg.DrawRectangle(Pens.Blue, rect);
            }
        }

        public void Clear()
        {
            this.Image = null;
            this.Frames.Clear();
            this.Refresh();
        }

        private void mnuAddFrame_Click(object sender, EventArgs e)
        {
            Frame frame = this.Selection;
            if (frame != null)
            {
                DrawXorFrame(frame.Rect);
                Rectangle unzoomRect = this.UnzoomRect(frame.Rect);
                this.Frames.Add(unzoomRect);
                this.Refresh();
                this.Selection = null;
            }
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            Frame frame = this.Selection;
            if (frame != null)
            {
                DrawXorFrame(frame.Rect);
                Rectangle unzoomRect = this.UnzoomRect(frame.Rect);
                Bitmap newimg = (new Bitmap(this.Image)).Clone(unzoomRect, this.Image.PixelFormat);
                this.Image = newimg;
                this.Frames = new FrameCollection<BarcodeFrame>();
                this.Selection = null;
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            Frame frame = this.Selection;
            if (frame != null)
            {
                DrawXorFrame(frame.Rect);
                this.Selection = null;
            }
        }
        /// <summary>
        /// 選択フレーム削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDelete_Click(object sender, EventArgs e)
        {
            var frms = this.Frames.Items.Where(x => x.IsSelected);

            foreach (BarcodeFrame frm in frms.ToArray())
            {
                this.Frames.Items.Remove(frm);
            }
            this.Refresh();
        }

      

    }
    /// <summary>
    ///領域抽出イベントのデータ
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        public BarcodeFrame Frame { get; private set; }
        public FrameEventArgs(BarcodeFrame frame)
        {
            this.Frame = frame;
        }
    }
}
