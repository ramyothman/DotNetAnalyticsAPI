using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupLibrary.Data
{
    public class DataItem
    {
        private string _category = "";
        public string Category
        {
            set { _category = value; }
            get { return _category; }
        }

        private ItemType _itemType;
        public ItemType ItemType
        {
            set { _itemType = value; }
            get { return _itemType; }
        }

        private string _name = "";
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        private string _webViewName = "";
        public string WebViewName
        {
            set { _webViewName = value; }
            get { return _webViewName; }
        }

        private string _appViewName = "";
        public string AppViewName
        {
            set { _appViewName = value; }
            get { return _appViewName; }
        }

        private string _dataType = "";
        public string DataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        private string _apiCommand = "";
        public string APICommand
        {
            set { _apiCommand = value; }
            get { return _apiCommand; }
        }

        private bool _allowedInSegments = true;
        public bool AllowedInSegments
        {
            set { _allowedInSegments = value; }
            get { return _allowedInSegments; }
        }

        private string _description = "";
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
    }
}
