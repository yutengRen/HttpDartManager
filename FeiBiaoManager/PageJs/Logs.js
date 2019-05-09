layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl', 'form', 'laydate'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl,
        form = layui.form(),
        laydate = layui.laydate;

    /*
    * 删除操作
    */
    $("#btn_del").on("click", function () {
        var ids = ComMethod.CheckItems();
        if (!ids) {
            layer.msg("请选择需要删除的数据！");
            return;
        }

        layer.confirm('确认删除？', {
            btn: ['确定', '取消']
        }, function () {
            var parms = { action: "Del", ids: ComMethod.CheckItems() };

            if (parms == null) return;

            $.post(SysConfig.API.GetSysLogData, parms, function (data) {

                if (data != null) {
                    if (data.Success) {
                        LoadPage(1);
                        $("#selected-all").prop("checked", "");
                        layer.msg("删除成功");
                    }
                    else {
                        layer.msg("删除失败：" + data.Msg);
                    }
                }
            }, "json");

        }, function () {
            layer.close();
        });
    });


    /*
* 搜索操作
*/
    form.on('submit(btn_search)', function (data) {
        LoadPage(1, data.field);
        return false;
    });


    /*
    * 分页搜索方法
    */
    var LoadPage = function (pageid, pdata) {
        var parms = { action: "GetLogList", pid: pageid, psize: 10 };

        ComMethod.AddGroupJson(parms, pdata);//叠加搜索参数

        $.post(SysConfig.API.GetSysLogData, parms, function (data) {

            laypage({
                cont: 'page',
                pages: data.PageCount //总页数
                          ,
                groups: 5 //连续显示分页数
                          ,
                curr: pageid,
                jump: function (obj, first) {
                    //得到了当前页，用于向服务端请求对应数据

                    var curr = obj.curr;
                    var getTpl = $("#demo").html();

                    laytpl(getTpl).render(data, function (html) {

                        $("#listdata").html(html);
                    });

                    //跳过第一页，从第二页开始按页码查询
                    if (!first) {
                        LoadPage(curr);
                    }
                }
            });

        }, "json");
    }

    //设置列表选择事件
    ComMethod.SetCheckList();

    //初始化列表分页数据
    LoadPage(1);

});