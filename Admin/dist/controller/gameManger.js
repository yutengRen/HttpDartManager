layui.define(function (exports) {
    var $ = layui.$
    , layer = layui.layer
    , laytpl = layui.laytpl
    , setter = layui.setter
    , view = layui.view
    , admin = layui.admin

    //公共业务的逻辑处理可以写在此处，切换任何页面都会执行
    //……

    //监听工具条
    //table.on('tool(LAY-user-back-manage)', function (obj) {
    //    var data = obj.data;
    //    if (obj.event === 'del') {
    //        layer.prompt({
    //            formType: 1
    //          , title: '敏感操作，请验证口令'
    //        }, function (value, index) {
    //            layer.close(index);
    //            layer.confirm('确定删除此管理员？', function (index) {
    //                console.log(obj)
    //                obj.del();
    //                layer.close(index);
    //            });
    //        });
    //    } else if (obj.event === 'edit') {
    //        admin.popup({
    //            title: '编辑管理员'
    //          , area: ['420px', '450px']
    //          , id: 'LAY-popup-user-add'
    //          , success: function (layero, index) {
    //              view(this.id).render('user/administrators/adminform', data).done(function () {
    //                  form.render(null, 'layuiadmin-form-admin');

    //                  //监听提交
    //                  form.on('submit(LAY-user-back-submit)', function (data) {
    //                      var field = data.field; //获取提交的字段

    //                      //提交 Ajax 成功后，关闭当前弹层并重载表格
    //                      //$.ajax({});
    //                      layui.table.reload('LAY-user-back-manage'); //重载表格
    //                      layer.close(index); //执行关闭 
    //                  });
    //              });
    //          }
    //        });
    //    }
    //});


    //监听搜索
    form.on('submit(LAY-user-front-search)', function (data) {
        var field = data.field;

        //执行重载
        table.reload('gamelist', {
            where: field
        });
    });

    //事件
    var active = {
        batchdel: function () {
            var checkStatus = table.checkStatus('gamelist')
            , checkData = checkStatus.data; //得到选中的数据

            if (checkData.length === 0) {
                return layer.msg('请选择数据');
            }

            layer.confirm('确定删除吗？', function (index) {
                var url = '../dist/Handler/managerDate.ashx';
                var parms = {};
                parms.action = "deleteGameName";
                parms.data = JSON.stringify(checkStatus.data);

                $.post(url, parms, function (data) {
                    if (data != null) {
                        if (data.Success) {
                            layer.msg(data.Msg);
                        }
                        else {
                            layer.msg(data.Msg);
                        }
                    }
                    else {
                        layer.msg("操作失败：数据为空");
                    }
                }, "json");

                table.reload('gamelist');
            });
        }
      , add: function () {
          admin.popup({
              title: '添加用户'
            , area: ['500px', '450px']
            , id: 'LAY-popup-user-add'
            , success: function (layero, index) {
                view(this.id).render('user/user/userform').done(function () {
                    form.render(null, 'layuiadmin-form-useradmin');

                    //监听提交
                    form.on('submit(LAY-user-front-submit)', function (data) {
                        var field = data.field; //获取提交的字段

                        //提交 Ajax 成功后，关闭当前弹层并重载表格
                        //$.ajax({});
                        layui.table.reload('LAY-user-manage'); //重载表格
                        layer.close(index); //执行关闭
                    });
                });
            }
          });
      }
    };


    //对外暴露的接口
    exports('gameManger', {});
});