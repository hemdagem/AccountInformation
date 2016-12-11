using System.Collections.Generic;
using System.Web.Mvc;

namespace Accounts.Web.Models
{
    public class SelectListModel
    {
        private IEnumerable<SelectListItem> _listItems;

        public string SelectedItem { get; set; }

        public IEnumerable<SelectListItem> ListItems
        {
            get { return new SelectList(_listItems, "Value", "Text"); }
            set { _listItems = value; }
        }
    }
}