using System;

namespace ReedBooks.Models.Database
{
    public class DependentDatabaseObject : DatabaseObject
    {
        private Guid _targetGuid;
        public Guid TargetGuid
        {
            get => _targetGuid;
            set
            {
                _targetGuid = value;
                OnPropertyChanged(nameof(TargetGuid));
            }
        }
    }
}
