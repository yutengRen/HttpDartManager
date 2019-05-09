layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl', 'form'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl,
        form = layui.form();

    $(document).ready(function () {
        var localTest = layui.data('user');
        //alert(localTest.username); //获得
        if (localTest.username == "" || localTest.username ==null || localTest.username ==undefined) {
            window.location.href = "/login.html";
        }
    })

    var LoadPage = function (pageid, pdata) {
        var parms = { action: "GetType" };

        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {

                    var dataList = data.Result;
                    $("#GoodsTypeIdss").append($("<option value=\"0\">－请选择－</option>"));
                    for (var i = 0; i < dataList.length; i++) {
                        $("#GoodsTypeIdss").append($("<option value=\"" + dataList[i].ID + "\">" + dataList[i].typeName + "</option>"));
                    }

                    form.render('select');//更新全部表单
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
        }, "json");
    }

    //初始化列表分页数据
    LoadPage(1);

    function getMachine() {
        var parms = { action: "GetMachine" };
        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    var dataList = data.Result;
                    $("#machineids").append($("<option value=\"0\">－请选择－</option>"));
                    for (var i = 0; i < dataList.length; i++) {
                        $("#machineids").append($("<option value=\"" + dataList[i].id + "\">" + dataList[i].machineNum + "</option>"));
                    }
                    form.render('select');//更新全部表单
                }
                else {
                    layer.msg(data.Msg);
                }
            }
        }, "json");
    }

    getMachine();

    form.on('select(GoodsTypeIdss)', function (data) {
        var pp = data.value;
        var parms = { action: "GetTypeName", id:pp};

        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {

                    var dataList = data.Result;
                    $("#TypeIdss").empty();
                    for (var i = 0; i < dataList.length; i++) {
                        $("#TypeIdss").append($("<option value=\"" + dataList[i].id + "\">" + dataList[i].gameName + "</option>"));
                    }

                    form.render('select');//更新全部表单
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
        }, "json");
    });

    //监听提交
    form.on('submit(btnAdd)', function (data) {
        //layer.msg(JSON.stringify(data.field));
        var localTest = layui.data('user');
        var parms = data.field;
        parms.action = "addGame";
        parms.username = localTest.username;
        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    layer.msg(data.Msg);
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
            else {
                layer.msg("添加失败：数据为空");
            }
        }, "json");
        return false;
    });
});