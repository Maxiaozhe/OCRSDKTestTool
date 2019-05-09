
//using FormSDKInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace OCRSDKTest
{
    public class CanvasPicture : PictureBox
    {
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem mnuAddFrame;
        private ToolStripMenuItem mnuCut;
        private ToolStripMenuItem mnuCancel;
        private ContextMenuStrip deleteMenuStrip;
        private ToolStripMenuItem mnuDelete;
        private ToolStripMenuItem mnuExtractFrame;
        private ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.IContainer components;
        private float realZoom = 1.0F;

        public event MouseEventHandler MouseSelectBegin;
        public event MouseEventHandler MouseSelectEnd;
        public event MouseEventHandler MouseSelectDrag;
        /// <summary>
        /// 領域抽出
        /// </summary>
        public event EventHandler<ExtractFrameEventArgs> ExtractFrame;

        public bool ShowRectCross { get; set; }

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

      

        public FrameCollection<Frame> Frames
        {
            get;
            set;
        }

        public List<Rectangle> SelectedFrames { get; set; }
        /// <summary>
        /// 特徴点情報
        /// </summary>
        public RectCrossInfo RectCrosses { get; set; }

        [System.ComponentModel.Browsable(false)]
        public List<TransArea> TranAreas { get; set; }

        public Frame Selection { get; set; }

        public CanvasPicture()
        {
            Frames = new FrameCollection<Frame>();
            this.ShowRectCross = false;
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddFrame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExtractFrame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.deleteMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddFrame,
            this.mnuExtractFrame,
            this.toolStripSeparator1,
            this.mnuCut,
            this.mnuCancel});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(146, 98);
            // 
            // mnuAddFrame
            // 
            this.mnuAddFrame.Name = "mnuAddFrame";
            this.mnuAddFrame.Size = new System.Drawing.Size(145, 22);
            this.mnuAddFrame.Text = "フレームの追加";
            this.mnuAddFrame.Click += new System.EventHandler(this.mnuAddFrame_Click);
            // 
            // mnuExtractFrame
            // 
            this.mnuExtractFrame.Name = "mnuExtractFrame";
            this.mnuExtractFrame.Size = new System.Drawing.Size(145, 22);
            this.mnuExtractFrame.Text = "領域抽出";
            this.mnuExtractFrame.Click += new System.EventHandler(this.mnuExtractFrame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(142, 6);
            // 
            // mnuCut
            // 
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.Size = new System.Drawing.Size(145, 22);
            this.mnuCut.Text = "トリミング";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Name = "mnuCancel";
            this.mnuCancel.Size = new System.Drawing.Size(145, 22);
            this.mnuCancel.Text = "キャンセル";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
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
            // CanvasPicture
            // 
            this.Click += new System.EventHandler(this.CanvasPicture_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasPicture_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CanvasPicture_MouseUp);
            this.contextMenu.ResumeLayout(false);
            this.deleteMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        private void CanvasPicture_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.AllowAddFrame || !this.ShowFrames)
            {
                return;
            }
            if (this.Image == null) return;
            Bitmap bmp = new Bitmap(this.Image);
            
            Point selPos = UnzoomPoint(new Point(e.X, e.Y));
            Rectangle rect = OcrSDK.GetFrameByPoint(bmp, selPos);
            this.Frames.Add(rect);
            this.Refresh();
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
                this.Frames.Draw(zg);
            }
            if (this.RectCrosses != null)
            {
                DrawRectCross(zg);
            }
            if (this.ShowFrames && this.TranAreas != null)
            {
                DrawTranArea(zg);
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


        public Rectangle UnzoomRect(RectangleF rectf)
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






        private void DrawRectCross(ZoomedGraphics zg)
        {
            if (!this.ShowRectCross)
            {
                return;
            }
            if (this.RectCrosses == null)
            {
                return;
            }
            if (this.RectCrosses.Rects.Count > 0)
            {
                zg.DrawRectangles( Pens.Red, this.RectCrosses.Rects.ToArray());
            }
            if (this.RectCrosses.Crosses.Count > 0)
            {
                foreach (Point pt in this.RectCrosses.Crosses)
                {
                    zg.DrawLine(Pens.Red, pt.X - 5, pt.Y, pt.X + 5, pt.Y);
                    zg.DrawLine(Pens.Red, pt.X, pt.Y - 5, pt.X, pt.Y + 5);
                }
            }

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
                    Point clientPos = this.PointToScreen(new Point(e.X, e.Y));
                    this.contextMenu.Show(clientPos);
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
            Frame selFrm = this.Frames.GetFrameByPoint(selPos.X, selPos.Y);
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
                this.Frames = new FrameCollection<Frame>();
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

            foreach (Frame frm in frms.ToArray())
            {
                this.Frames.Items.Remove(frm);
            }
            this.Refresh();
        }

        /// <summary>
        /// 領域抽出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExtractFrame_Click(object sender, EventArgs e)
        {
            Frame frame = this.Selection;
            if (frame != null)
            {
                Rectangle selRect = frame.Rect;
                DrawXorFrame(selRect);
                this.Selection = null;
                if (this.ExtractFrame != null)
                {
                    
                    this.ExtractFrame.Invoke(sender, new ExtractFrameEventArgs(this.UnzoomRect(selRect)));
                }
            }
        }

        private void CanvasPicture_Click(object sender, EventArgs e)
        {
            this.Focus();
        }
    }
    /// <summary>
    ///領域抽出イベントのデータ
    /// </summary>
    public class ExtractFrameEventArgs : EventArgs
    {
        public Rectangle Selection { get; private set; }
        public ExtractFrameEventArgs(Rectangle selection)
        {
            this.Selection = selection;
        }
    }

}
