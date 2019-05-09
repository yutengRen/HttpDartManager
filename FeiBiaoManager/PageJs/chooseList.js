layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl', 'form'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl,
        form = layui.form();
        //table = layui.table;

    /*
    * 分页搜索方法
    */
    var LoadPage = function (pageid, pdata) {
        var parms = { action: "getChooseList", pid: pageid, psize: 10 };

        //ComMethod.AddGroupJson(parms, pdata);//叠加搜索参数

        $.post(SysConfig.API.managerDate, parms, function (data) {

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

                    form.render();
                    //跳过第一页，从第二页开始按页码查询
                    if (!first) {
                        LoadPage(curr);
                        form.render();
                    }
                }
            });

        }, "json");
    }

    //设置列表选择事件
    //ComMethod.SetCheckList();

    //初始化列表分页数据
    LoadPage(1);

    //监听提交
    form.on('submit(btnAdd)', function (data) {
        //layer.msg(JSON.stringify(data.field));
        //var checkStatus = table.checkStatus('userTable'), data = checkStatus.data;
        //layer.alert(JSON.stringify(data));

        var ids = ComMethod.CheckItems();
        if (!ids) {
            layer.msg("请选择需要修改的数据！");
        }
        else {
            //alert(ids);
        }

        var parms = data.field;
        parms.action = "starGame";
        parms.id = ids;
        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    layer.msg(data.Msg);
                }
                else {
                    layer.msg(data.Msg);
                }
            }
            else {
                layer.msg("添加失败：数据为空");
            }
        }, "json");
        return false;
    });

    //layui.use('table', function () {
    //    var table = layui.table;

    //    table.render({
    //        elem: '#test'
    //    , url: SysConfig.API.managerDate
    //    , cols: [[
    //    { type: 'checkbox' }
    //    , { field: 'id', width: 80, title: 'ID', sort: true }
    //    , { field: 'username', width: 80, title: '用户名' }
    //    , { field: 'sex', width: 80, title: '性别', sort: true }
    //    , { field: 'city', width: 80, title: '城市' }
    //    , { field: 'sign', title: '签名', minWidth: 100 }
    //    , { field: 'experience', width: 80, title: '积分', sort: true }
    //    , { field: 'score', width: 80, title: '评分', sort: true }
    //    , { field: 'classify', width: 80, title: '职业' }
    //    , { field: 'wealth', width: 135, title: '财富', sort: true }
    //    ]]
    //    , page: true
    //    });
    //});
});