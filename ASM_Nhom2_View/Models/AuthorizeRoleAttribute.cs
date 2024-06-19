using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeRoleAttribute : ActionFilterAttribute
{
    private readonly int _roleId;

    public AuthorizeRoleAttribute(int roleId)
    {
        _roleId = roleId;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var roleId = session.GetInt32("RoleId");

        if (roleId == null || roleId != _roleId)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
        }

        base.OnActionExecuting(context);
    }
}
