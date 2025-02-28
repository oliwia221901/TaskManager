﻿using MediatR;
using TaskManagerAPI.Application.Dtos.PermissionsManage.CreatePermission;

namespace TaskManagerAPI.Application.PermissionsManage.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<int>
	{
        public string UserId { get; set; }
        public CreatePermissionDto CreatePermissionDto { get; set; }
    }
}
