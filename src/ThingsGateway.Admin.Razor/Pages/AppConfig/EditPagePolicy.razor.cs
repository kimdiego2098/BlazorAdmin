//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
//  Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Forms;

using ThingsGateway.Admin.Application;
using ThingsGateway.Extension.Generic;
using ThingsGateway.NewLife.Extension;

namespace ThingsGateway.Admin.Razor;

public partial class EditPagePolicy
{
    [Parameter]
    [EditorRequired]
    public PagePolicy Model { get; set; }

    [Parameter]
    [EditorRequired]
    public Func<EditContext, Task> OnSave { [return: NotNull] get; set; }

    private List<TreeViewItem<SysResource>> ShortcutsTreeViewItems;
    private List<TreeViewItem<SysResource>> RazorTreeViewItems;

    protected override Task OnParametersSetAsync()
    {
        ShortcutsTreeViewItems = ResourceUtil.BuildTreeItemList(AppContext.OwnMenus.WhereIf(!ShortcutsSearchText.IsNullOrEmpty(), a => a.Title.Contains(ShortcutsSearchText)), Model.Shortcuts, null);
        RazorTreeViewItems = ResourceUtil.BuildTreeItemList(AppContext.OwnMenus.WhereIf(!RazorSearchText.IsNullOrEmpty(), a => a.Title.Contains(RazorSearchText)), new List<long>() { Model.Razor }, null, disableFunc: (a => a.Href.IsNullOrEmpty()));
        return base.OnParametersSetAsync();
    }

    private static bool ModelEqualityComparer(SysResource x, SysResource y) => x.Id == y.Id;

    private Task OnShortcutsTreeItemChecked(List<TreeViewItem<SysResource>> items)
    {
        Model.Shortcuts = items.Select(a => a.Value).Where(a => !a.Href.IsNullOrEmpty()).Select(a => a.Id).ToList();
        StateHasChanged();
        return Task.CompletedTask;
    }
    private async Task OnRazorTreeItemClick(TreeViewItem<SysResource> item, ISelectObjectContext<long> context)
    {
        Model.Razor = item.Value.Id;
        StateHasChanged();
        await context.CloseAsync();
    }

    private string ShortcutsSearchText;

    private async Task<List<TreeViewItem<SysResource>>> OnShortcutsClickSearch(string searchText)
    {
        await Task.CompletedTask;
        ShortcutsSearchText = searchText;
        return ResourceUtil.BuildTreeItemList(AppContext.OwnMenus.WhereIf(!ShortcutsSearchText.IsNullOrEmpty(), a => a.Title.Contains(ShortcutsSearchText)), Model.Shortcuts, null);

    }
    private string RazorSearchText;
    private async Task<List<TreeViewItem<SysResource>>> OnRazorClickSearch(string searchText)
    {
        await Task.CompletedTask;
        RazorSearchText = searchText;
        return  ResourceUtil.BuildTreeItemList(AppContext.OwnMenus.WhereIf(!RazorSearchText.IsNullOrEmpty(), a => a.Title.Contains(RazorSearchText)), new List<long>() { Model.Razor }, null, disableFunc: (a => a.Href.IsNullOrEmpty())).ToList();

    }
}
