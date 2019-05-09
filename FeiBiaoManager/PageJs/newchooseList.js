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
    var LoadPage = function () {
        var table = layui.table;

        table.render({
            elem: '#test'
        , url: SysConfig.API.managerDate
        , parseData: function (res) { //res 即为原始返回的数据
            return {
                //"code": res.status, //解析接口状态
                //"msg": res.message, //解析提示文本
                "RowCount": res.total, //解析数据长度
                "Result": res.data.item //解析数据列表
            };
        }
        , cols: [[
        { type: 'checkbox' }
        , { field: 'id', width: 80, title: 'ID', sort: true }
        , { field: 'username', width: 80, title: '用户名' }
        , { field: 'sex', width: 80, title: '性别', sort: true }
        , { field: 'city', width: 80, title: '城市' }
        , { field: 'sign', title: '签名', minWidth: 100 }
        , { field: 'experience', width: 80, title: '积分', sort: true }
        , { field: 'score', width: 80, title: '评分', sort: true }
        , { field: 'classify', width: 80, title: '职业' }
        , { field: 'wealth', width: 135, title: '财富', sort: true }
        ]]
        , page: true
        });
    }

    //设置列表选择事件
    //ComMethod.SetCheckList();

    //初始化列表分页数据
    //LoadPage();

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

    layui.use('table', function () {
        var table = layui.table;

        table.render({
            elem: '#test'
        , url: SysConfig.API.managerDate
        , parseData: function (res) { //res 即为原始返回的数据
            return {
                //"code": res.status, //解析接口状态
                //"msg": res.message, //解析提示文本
                "RowCount": res.total, //解析数据长度
                "Result": res.data.item //解析数据列表
            };
        }
        , cols: [[
        { type: 'checkbox' }
        , { field: 'id', width: 80, title: 'ID', sort: true }
        , { field: 'username', width: 80, title: '用户名' }
        , { field: 'sex', width: 80, title: '性别', sort: true }
        , { field: 'city', width: 80, title: '城市' }
        , { field: 'sign', title: '签名', minWidth: 100 }
        , { field: 'experience', width: 80, title: '积分', sort: true }
        , { field: 'score', width: 80, title: '评分', sort: true }
        , { field: 'classify', width: 80, title: '职业' }
        , { field: 'wealth', width: 135, title: '财富', sort: true }
        ]]
        , page: true
        });
    });
});