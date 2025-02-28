﻿using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Domain.Entities.PermissionManage
{
    public class Permission
	{
        public int PermissionId { get; set; }
        public string UserId { get; set; }
        public int TaskListId { get; set; }
        public int? TaskItemId { get; set; }
        public PermissionLevel Level { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
    }
}
