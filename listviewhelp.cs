using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motapart_Core
{
    public static class ListViewExtensions
    {
        public static void MakeColumnHeaders(
            this ListView lvw, params object[] header_info)
        {
            lvw.Columns.Clear();

            for (int i = header_info.GetLowerBound(0);
                     i <= header_info.GetUpperBound(0);
                     i += 3)
            {
                lvw.Columns.Add(
                    (string)header_info[i],
                    (int)header_info[i + 1],
                    (HorizontalAlignment)header_info[i + 2]);
            }
        }
        public static void AddRow(this ListView lvw, int image_index,
            string item_title, params string[] subitem_titles)
        {
            ListViewItem new_item = lvw.Items.Add(item_title, 1);

            new_item.ImageIndex = image_index;

            for (int i = subitem_titles.GetLowerBound(0);
                     i <= subitem_titles.GetUpperBound(0);
                     i++)
            {
                new_item.SubItems.Add(subitem_titles[i]);
            }
        }
    }
}
