﻿//------------------------------------------------------------------------------
//  此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
//  此代码版权（除特别声明外的代码）归作者本人Diego所有
//  源代码使用协议遵循本仓库的开源协议及附加协议
//  Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
//  Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
//  使用文档：https://thingsgateway.cn/
//  QQ群：605534569
//------------------------------------------------------------------------------

using SqlSugar;

using ThingsGateway.Admin.Application;
using ThingsGateway.NewLife.Extension;

namespace ThingsGateway.Admin.Razor;

public partial class PositionTree : IDisposable
{
    [Parameter]
    [NotNull]
    public long Value { get; set; }

    [Parameter]
    public Func<long, Task> ValueChanged { get; set; }

    [NotNull]
    private List<TreeViewItem<PositionTreeOutput>> Items { get; set; }

    [Inject]
    private IStringLocalizer<ThingsGateway.Admin.Razor._Imports> AdminLocalizer { get; set; }
    [Inject]
    [NotNull]
    private ISysPositionService? SysPositionService { get; set; }

    private bool ModelEqualityComparer(PositionTreeOutput x, PositionTreeOutput y) => x.Id == y.Id;

    private async Task OnTreeItemClick(TreeViewItem<PositionTreeOutput> item)
    {
        var value = item.Value.Id;
        Value = value;
        if (ValueChanged != null && item.Value.IsPosition)
        {
            await ValueChanged.Invoke(value);
        }
    }

    private List<TreeViewItem<PositionTreeOutput>> ZItem;
    protected override async Task OnInitializedAsync()
    {
        ZItem = new List<TreeViewItem<PositionTreeOutput>>() {new TreeViewItem<PositionTreeOutput>(new PositionTreeOutput(){ IsPosition=true})
        {
            Text = AdminLocalizer["All"],
            IsActive = Value == 0,
            IsExpand = false,
            CheckedState = Value == 0 ? CheckboxState.Checked : CheckboxState.UnChecked
        } };
        var items = (await SysPositionService.TreeAsync());
        Items = ZItem.Concat(PositionUtil.BuildTreeItemList(items, new List<long> { Value })).ToList();

        context = ExecutionContext.Capture();
        DispatchService.Subscribe(Refresh);
        await base.OnInitializedAsync();
    }
    private ExecutionContext? context;
    private async Task Notify()
    {
        var current = ExecutionContext.Capture();
        try
        {
            ExecutionContext.Restore(context);
            await InvokeAsync(async () =>
            {
                await OnClickSearch(SearchText);
            });
        }
        finally
        {
            ExecutionContext.Restore(current);
        }
    }
    private async Task Refresh(DispatchEntry<SysPosition> entry)
    {
        await Notify();
    }

    [Inject]
    private IDispatchService<SysPosition> DispatchService { get; set; }
    private string SearchText;

    private async Task OnClickSearch(string searchText)
    {
        SearchText = searchText;
        var items = (await SysPositionService.TreeAsync());
        items = items.WhereIF(!searchText.IsNullOrEmpty(), a => a.Name.Contains(searchText)).ToList();
        Items = ZItem.Concat(PositionUtil.BuildTreeItemList(items, new List<long> { Value })).ToList();

        StateHasChanged();
    }

    public void Dispose()
    {
        DispatchService.UnSubscribe(Refresh);
    }
}
