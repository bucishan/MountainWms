/**
 * AdminLTE 无刷新导航 和UI设置相关
 */
$(function () {
    'use strict'
    var requestUrl = '';
    var requestStatus = false;
    var requestLoading;

    /**
     * 主菜单局部刷新事件 重写AdminLte 插件
     */
    var Tree = $.fn.tree.Constructor;
    Tree.prototype._setUpListeners = function () {
        var that = this;
        $(this.element).on('click', this.options.trigger, function (event) {
            if ($($(this)[0]).hasClass("child-link")) {
                //that.toggle($(this), event);
                mainMenuClickFunc.call(this);
            } else {
                that.toggle($(this), event)
            }
        })
    };
    function mainMenuClickFunc() {
        var baseurl = window.location.protocol + '//' + window.location.host;
        var controller = $(this).attr("menu-controller");
        if (requestStatus == true && requestUrl == controller) {
            //console.log('熔断');
            //layer.msg('test', { icon: 5});
            return;
        }

        requestUrl = controller;
        requestStatus = true;
        requestLoading = layer.load(0, {
            shade: [0.3, '#222']
        });

        $(".sidebar-menu .treeview li").removeClass("active");
        $($(this).parent()).addClass("active");

        if (!$(this).offsetParent().hasClass("active")) {
            $(".sidebar-menu .treeview").removeClass("active");
            $(this).offsetParent().addClass("active");
        }


        $.ajax({
            url: baseurl + controller,
            //type: "POST",
            data: {},
            cache: false,
            success: (data) => {
                var view = $(data);
                //清理过期的[css,js,datetimepicker]
                $("link[name=section_css]").remove();
                $("script[name=section_js]").remove();
                $(".datetimepicker").remove();

                //获取模板中的相关内容
                var styles = view.filter('link');
                var scripts = view.filter('script');

                //动态添加CSS库
                if (styles.length > 0) {
                    styles.attr('name', 'section_css');
                    $("head").append(styles);
                }

                //添加页面节点
                $(".partial_view").html(view.filter('view').html());

                //动态添加JS库
                if (scripts.length > 0) {
                    scripts.attr('name', 'section_js');
                    $("body").append(scripts);
                }
                requestStatus = false;
                requestUrl = '';
                layer.close(requestLoading);
            },
            error: (error) => {
                requestStatus = false;
                requestUrl = '';
                layer.close(requestLoading);
                layer.msg(error.status + '--' + error.statusText, { icon: 5, time: 5000 });
                console.error(error.responseText);
            }
        });
    }

    /**
     * Get access to plugins
     */

    $('[data-toggle="control-sidebar"]').controlSidebar()
    $('[data-toggle="push-menu"]').pushMenu()
    var $pushMenu = $('[data-toggle="push-menu"]').data('lte.pushmenu')
    var $controlSidebar = $('[data-toggle="control-sidebar"]').data('lte.controlsidebar')
    var $layout = $('body').data('lte.layout')
    $(window).on('load', function () {
        // Reinitialize variables on load
        $pushMenu = $('[data-toggle="push-menu"]').data('lte.pushmenu')
        $controlSidebar = $('[data-toggle="control-sidebar"]').data('lte.controlsidebar')
        $layout = $('body').data('lte.layout')
    })

    /**
     * List of all the available skins
     *
     * @type Array
     */
    var mySkins = [
        'skin-blue',
        'skin-black',
        'skin-red',
        'skin-yellow',
        'skin-purple',
        'skin-green',
        'skin-blue-light',
        'skin-black-light',
        'skin-red-light',
        'skin-yellow-light',
        'skin-purple-light',
        'skin-green-light'
    ]

    /**
     * 获取localStore信息储存
     *
     * @param String name Name of of the setting
     * @returns String The value of the setting | null
     */
    function get(name) {
        if (typeof (Storage) !== 'undefined') {
            return localStorage.getItem(name)
        } else {
            window.alert('请使用现代浏览器正确查看此模板！')
        }
    }

    /**
     * 添加localStore信息储存
     *
     * @param String name Name of the setting
     * @param String val Value of the setting
     * @returns void
     */
    function store(name, val) {
        if (typeof (Storage) !== 'undefined') {
            localStorage.setItem(name, val)
        } else {
            window.alert('请使用现代浏览器正确查看此模板！')
        }
    }

    /**
     * 切换布局
     *
     * @param String cls the layout class to toggle
     * @returns void
     */
    function changeLayout(cls) {
        $('body').toggleClass(cls)
        $layout.fixSidebar()
        if ($('body').hasClass('fixed') && cls == 'fixed') {
            $pushMenu.expandOnHover()
            $layout.activate()
        }
        $controlSidebar.fix()
        var val = get(cls);
        var valbool = false;
        if (val != undefined && val!='') {
            valbool = val == 'true';
        }
        store(cls, !valbool)
    }

    /**
     * Replaces the old skin with the new skin
     * @param String cls the new skin class
     * @returns Boolean false to prevent link's default action
     */
    function changeSkin(cls) {
        $.each(mySkins, function (i) {
            $('body').removeClass(mySkins[i])
        })

        $('body').addClass(cls)
        store('skin', cls)
        return false
    }

    /**
     * Retrieve default settings and apply them to the template
     *
     * @returns void
     */
    function setup() {
        var storeskin = get('skin')
        if (storeskin && $.inArray(storeskin, mySkins))
            changeSkin(storeskin)

        // 皮肤切换事件监听
        $('[data-skin]').on('click', function (e) {
            if ($(this).hasClass('knob'))
                return
            e.preventDefault()
            changeSkin($(this).data('skin'))
        })

        // 布局操作事件监听
        $('[data-layout]').on('click', function () {
            changeLayout($(this).data('layout'))
        })

        $('[data-controlsidebar]').on('click', function () {
            changeLayout($(this).data('controlsidebar'))
            var slide = !$controlSidebar.options.slide
            $controlSidebar.options.slide = slide
            if (!slide)
                $('.control-sidebar').removeClass('control-sidebar-open')
        })

        $('[data-sidebarskin="toggle"]').on('click', function () {
            var $sidebar = $('.control-sidebar')
            if ($sidebar.hasClass('control-sidebar-dark')) {
                $sidebar.removeClass('control-sidebar-dark')
                $sidebar.addClass('control-sidebar-light')
            } else {
                $sidebar.removeClass('control-sidebar-light')
                $sidebar.addClass('control-sidebar-dark')
            }
        })

        $('[data-enable="expandOnHover"]').on('click', function () {
            $(this).attr('disabled', true)
            $pushMenu.expandOnHover()
            if (!$('body').hasClass('sidebar-collapse'))
                $('[data-layout="sidebar-collapse"]').click()
        })

        //设置控制属性
        //固定布局
        var storelayout = get('fixed')
        if (storelayout && storelayout == 'true')
            $('body').addClass('fixed');
        if ($('body').hasClass('fixed')) {
            $('[data-layout="fixed"]').attr('checked', 'checked')
        }
        //装箱布局
        storelayout = get('layout-boxed')
        if (storelayout && storelayout == 'true')
            $('body').addClass('layout-boxed');
        if ($('body').hasClass('layout-boxed')) {
            $('[data-layout="layout-boxed"]').attr('checked', 'checked')
        }
        //切换侧边栏
        storelayout = get('sidebar-collapse')
        if (storelayout && storelayout == 'true')
            $('body').addClass('sidebar-collapse');
        if ($('body').hasClass('sidebar-collapse')) {
            $('[data-layout="sidebar-collapse"]').attr('checked', 'checked')
        }
        //切换右边栏幻灯片
        storelayout = get('control-sidebar-open')
        if (storelayout && storelayout == 'true')
            $('body').addClass('control-sidebar-open');
        if ($('body').hasClass('control-sidebar-open')) {
            $('[data-controlsidebar="control-sidebar-open"]').attr('checked', 'checked')
        }
    }
    setup()
    $('[data-toggle="tooltip"]').tooltip()
})