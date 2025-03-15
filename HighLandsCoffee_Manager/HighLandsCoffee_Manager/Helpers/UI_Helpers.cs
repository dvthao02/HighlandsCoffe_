using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HighLandsCoffee_Manager.Helpers
{
    public static class UI_Helpers
    {
        private static SimpleButton currentButton = null; // Nút hiện tại đang được active
        private static SimpleButton currentChildButton = null; // Active Button của ChildForm

        private static class CustomColors
        {
            // Main Form
            public static Color MainFormBackColor = Color.FromArgb(240, 240, 240);
            public static Color MainFormForeColor = Color.Black;

            // Child Form
            public static Color ChildFormBackColor = Color.FromArgb(153, 102, 51); // Nâu sáng
            public static Color ChildFormForeColor = Color.FromArgb(255, 235, 205); // Vàng sáng
            public static Color ChildButtonDefaultBackColor = Color.FromArgb(123, 63, 0);
            public static Color ChildButtonHoverBackColor = Color.FromArgb(255, 87, 51);
            public static Color ChildButtonActiveBackColor = Color.FromArgb(255, 43, 43);
            public static Color ChildButtonHoverForeColor = Color.White;
            public static Color ChildButtonActiveForeColor = Color.White;

            // Buttons (Main Form)
            public static Color DefaultBackColor = Color.FromArgb(167, 7, 10); // Màu đỏ mặc định
            public static Color DefaultForeColor = Color.FromArgb(230, 230, 230);
            public static Color ActiveBackColor = Color.FromArgb(235, 0, 4); // Màu khi active
            public static Color ActiveForeColor = Color.FromArgb(255, 255, 255); // Màu chữ khi active

            // TextEdit
            public static Color TextEditBackColor = Color.WhiteSmoke;
            public static Color TextEditForeColor = Color.Black;

            // Fonts
            public static Font DefaultFont = new Font("Segoe UI", 10);
            public static Font HeaderFont = new Font("Segoe UI", 14, FontStyle.Bold);
        }

        // ============================= MAIN FORM =============================

        // Hàm active nút, thay đổi màu sắc ngay lập tức
        public static void ActivateButton(SimpleButton button)
        {
            if (currentButton != null && currentButton != button)
            {
                // Reset nút trước đó trước khi active nút mới
                ResetButtonAppearance(currentButton);
            }

            currentButton = button;
            button.LookAndFeel.UseDefaultLookAndFeel = false;  // Tắt look and feel mặc định

            // Áp dụng màu khi nút được active
            button.Appearance.BackColor = CustomColors.ActiveBackColor;
            button.Appearance.ForeColor = CustomColors.ActiveForeColor;
            button.RightToLeft = RightToLeft.Yes;
        }

        // Hàm khi mouse hover vào nút chính
        public static void ApplyMainButtonHover(SimpleButton button)
        {
            button.MouseEnter += (s, e) =>
            {
                if (button != currentButton) // Không thay đổi nút active
                {
                    button.Appearance.BackColor = CustomColors.ActiveBackColor;
                    button.Appearance.ForeColor = CustomColors.ActiveForeColor;
                    button.Invalidate();
                }
            };

            button.MouseLeave += (s, e) =>
            {
                if (button != currentButton) // Không thay đổi nút active
                {
                    ResetButtonAppearance(button);
                }
            };
        }

        // Reset lại màu sắc cho nút
        public static void ResetButtonAppearance(SimpleButton button)
        {
            if (button != currentButton) // Không reset nút đang active
            {
                button.LookAndFeel.UseDefaultLookAndFeel = false;
                button.Appearance.BackColor = CustomColors.DefaultBackColor;
                button.Appearance.ForeColor = CustomColors.DefaultForeColor;
                button.RightToLeft = RightToLeft.No;
            }
        }

        // ============================= CHILD FORM =============================

        // Hàm active nút của child form
        public static void ActivateChildButton(SimpleButton button)
        {
            if (currentChildButton != null && currentChildButton != button)
                ResetChildButtonAppearance(currentChildButton);

            currentChildButton = button;

            button.LookAndFeel.UseDefaultLookAndFeel = false;
            button.Appearance.BackColor = CustomColors.ChildButtonActiveBackColor;
            button.Appearance.ForeColor = CustomColors.ChildButtonActiveForeColor;
            button.Invalidate();
        }

        // Reset lại màu sắc cho nút của child form
        public static void ResetChildButtonAppearance(SimpleButton button)
        {
            if (button != currentChildButton) // Không reset nút child active
            {
                button.LookAndFeel.UseDefaultLookAndFeel = false;
                button.Appearance.BackColor = CustomColors.ChildButtonDefaultBackColor;
                button.Appearance.ForeColor = CustomColors.DefaultForeColor;
                button.Invalidate();
            }
        }

        // Hàm khi mouse hover vào nút của child form
        public static void ApplyChildButtonHover(SimpleButton button)
        {
            button.MouseEnter += (s, e) =>
            {
                if (button != currentChildButton)
                {
                    button.Appearance.BackColor = CustomColors.ChildButtonHoverBackColor;
                    button.Appearance.ForeColor = CustomColors.ChildButtonHoverForeColor;
                    button.Invalidate();
                }
            };

            button.MouseLeave += (s, e) =>
            {
                if (button != currentChildButton)
                {
                    ResetChildButtonAppearance(button);
                }
            };
        }

        // Áp dụng style cho form con
        public static void ApplyChildFormStyle(Form childForm)
        {
            childForm.BackColor = CustomColors.ChildFormBackColor;
            childForm.ForeColor = CustomColors.ChildFormForeColor;

            foreach (Control control in childForm.Controls)
            {
                if (control is SimpleButton button)
                {
                    ResetChildButtonAppearance(button);
                    ApplyChildButtonHover(button);
                }
                else if (control is TextEdit textEdit)
                {
                    ApplyTextEditStyle(textEdit);
                }
                else if (control is Guna2HtmlLabel label)
                {
                    ApplyHeaderStyle(label);
                }
            }
        }

        // ============================= COMPONENT STYLES =============================

        // Áp dụng style cho các Label Header
        public static void ApplyHeaderStyle(Guna2HtmlLabel label)
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = CustomColors.MainFormForeColor;
            label.Font = CustomColors.HeaderFont;
            label.AutoSize = false;
            label.Size = new Size(800, 40);
            label.TextAlignment = ContentAlignment.MiddleCenter;
        }

        // Áp dụng style cho các TextEdit
        public static void ApplyTextEditStyle(TextEdit textEdit)
        {
            textEdit.BackColor = CustomColors.TextEditBackColor;
            textEdit.ForeColor = CustomColors.TextEditForeColor;
            textEdit.Font = CustomColors.DefaultFont;
            textEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        // Tùy chỉnh giao diện cho Guna2DataGridView
        public static void CustomizeGridViewAppearance(Guna2DataGridView gridView)
        {
            gridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            gridView.ColumnHeadersHeight = 35;
            gridView.ReadOnly = true;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;
            gridView.EnableHeadersVisualStyles = false;
            gridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridView.RowHeadersVisible = false;
            gridView.ScrollBars = ScrollBars.Both;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                column.MinimumWidth = 100;
            }

            gridView.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            gridView.DefaultCellStyle.ForeColor = Color.Black;
            gridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            gridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
        }
    }
}
