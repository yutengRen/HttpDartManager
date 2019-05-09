layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl', 'form'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl,
        form = layui.form();

    var index = parent.layer.getFrameIndex(window.name);//获取父窗体
    var KeyId = parent.$("#KeyId").val();//获取修改Id，隐藏域

    //监听提交
    form.on('submit(btn_sub)', function (data) {
        //编辑，包含新增修改
        var parms = { action: "AddApp", ApplicationId: KeyId };
        ComMethod.AddGroupJson(parms, data.field);
 
        if (parms == null) return;
        $.post(SysConfig.API.SysApplicationData, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    parent.layer.close(index);//关闭窗口                             
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
        }, "json");

        return false;
    });


    /*
    * 分页搜索方法
    */
    var LoadPage = function (pageid, pdata) {
        var parms = { action: "GetParentList" };

        $.post(SysConfig.API.SysApplicationData, parms, function (data) {

            var getTpl = $("#ptselect").html();

            laytpl(getTpl).render(data, function (html) {
         
                $("#ParentId").html(html);
                form.render('select');
                View();
            });

        }, "json");
    }

    var View = function () {
        //编辑时查询显示表单
        if (KeyId) {
            var parms = { action: "View", Id: KeyId };

            $.post(SysConfig.API.SysApplicationData, parms, function (data) {
                if (data != null) {
                    if (data.Success) {
                        //显示表单值
                        ComMethod.SetFormData("form1", data.Result);
                        form.render('select');//更新全部表单
                    }
                    else {
                        layer.msg("添加失败：" + data.Msg);
                    }
                }
            }, "json");
        }
    }


    //初始化列表分页数据
    LoadPage(1);
   
    
    
    

});