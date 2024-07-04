using ToolExportVideo.Library;

namespace ToolExportVideo.Models
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public EditMode EditMode { get; set; }
        /// <summary>
        /// hàm set giá trị cho property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetValue(string propertyName, object value)
        {
            this.GetType().GetProperty(propertyName)?.SetValue(this, value);
        }
        /// <summary>
        /// Hàm lấy giá trị theo tên thuộc tính
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object? GetValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName)?.GetValue(this, null);
        }
    }
}