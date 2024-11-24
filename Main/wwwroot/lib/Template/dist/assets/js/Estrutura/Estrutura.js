// Estrutura


// Barra Lateral e Logo
function AGILBSLatNavbar(){
   
    $('.AGILBSLatNavbar').html(
        
        
        // Logo da Plataforma | AGILBS
        '<div class="kt-aside__brand kt-grid__item " id="kt_aside_brand">'+
            '<div class="kt-aside__brand-logo">'+
                '<a href="index.html">'+
                    '<img alt="Logo" src="assets/media/logos/logo-6.png" />'+
                '</a>'+
            '</div>'+
            '<div class="kt-aside__brand-tools">'+
                '<button class="kt-aside__brand-aside-toggler kt-aside__brand-aside-toggler--left" id="kt_aside_toggler"><span></span></button>'+
            '</div>'+
        '</div>'+
        // Acaba Logo da Plataforma | AGILBS

        
        // Menu De Navegação Lateral(Esquerda)
        '<div class="kt-aside-menu-wrapper kt-grid__item kt-grid__item--fluid" id="kt_aside_menu_wrapper">'+
            '<div id="kt_aside_menu" class="kt-aside-menu " data-ktmenu-vertical="1" data-ktmenu-scroll="1" data-ktmenu-dropdown-timeout="500">'+
                
                '<ul class="kt-menu__nav AGILBSNavbarMenu"></ul>'+
            '</div>'+
        '</div>'+
        // ACABA Menu De Navegação Lateral(Esquerda)

       
        // MENU NO RODAPE DA NAVBAR
        '<div class="kt-aside__footer kt-grid__item" id="kt_aside_footer" style="display:none;">'+
            '<div class="kt-aside__footer-nav AGILBSNavbarMenuFooter"></div>'+
        '</div>'
        // ACABA MENU NO RODAPE DA NAVBAR       
    )

    // Carrega links de navegação
    AGILBSLinksNavegacao();

    // Carrega Links do Rodape - Desactivado
    //AGILBSLatNavbarFooter()
}
AGILBSLatNavbar();




// Links de Nagevação
function AGILBSLinksNavegacao(){
    
    $('.AGILBSLatNavbar .kt-menu__nav.AGILBSNavbarMenu').html(

        '<li class="kt-menu__section ">'+
            '<h4 class="kt-menu__section-text">Módulos</h4>'+
            '<i class="kt-menu__section-icon flaticon-more-v2"></i>'+
        '</li>'+
        
        //<!-- Dashboards-->
        '<li class="kt-menu__item  kt-menu__item--submenu kt-menu__item--open kt-menu__item--here" aria-haspopup="true" data-ktmenu-submenu-toggle="hover"><a href="javascript:;" class="kt-menu__link kt-menu__toggle"><i class="kt-menu__link-icon flaticon2-graphic"></i><span class="kt-menu__link-text">Dashboards</span><i class="kt-menu__ver-arrow la la-angle-right"></i></a>'+
            '<div class="kt-menu__submenu "><span class="kt-menu__arrow"></span>'+
                '<ul class="kt-menu__subnav">'+											
                    '<li class="kt-menu__item  kt-menu__item--parent" aria-haspopup="true"><span class="kt-menu__link"><span class="kt-menu__link-text">Dashboards</span></span></li>'+
                    '<li class="kt-menu__item kt-menu__item--active" aria-haspopup="true"><a href="index.html" class="kt-menu__link "><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Geral</span></a></li>'+											
                '</ul>'+
            '</div>'+
        '</li>'+

        //<!-- Catalogo Artigos-->
        '<li class="kt-menu__item  kt-menu__item--submenu" aria-haspopup="true" data-ktmenu-submenu-toggle="hover">'+
            '<a href="javascript:;" class="kt-menu__link kt-menu__toggle"><i class="kt-menu__link-icon flaticon2-tag"></i><span class="kt-menu__link-text">Catálogo de Artigos</span><i class="kt-menu__ver-arrow la la-angle-right"></i></a>'+
            '<div class="kt-menu__submenu "><span class="kt-menu__arrow"></span>'+
                '<ul class="kt-menu__subnav">'+										
                    '<li class="kt-menu__item  kt-menu__item--parent" aria-haspopup="true"><span class="kt-menu__link"><span class="kt-menu__link-text">Catálogo de Artigos</span></span></li>'+
                    '<li class="kt-menu__item" aria-haspopup="true"><a href="index.html" class="kt-menu__link "><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Artigos/Conjuntos</span></a></li>'+
                    '<li class="kt-menu__item" aria-haspopup="true"><a href="index.html" class="kt-menu__link "><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Categorias</span></a></li>'+
                    '<li class="kt-menu__item" aria-haspopup="true"><a href="index.html" class="kt-menu__link "><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Marcas</span></a></li>'+											
                '</ul>'+
            '</div>'+
        '</li>'+

        '<li class="kt-menu__section ">'+
            '<h4 class="kt-menu__section-text">Administração</h4>'+
            '<i class="kt-menu__section-icon flaticon-more-v2"></i>'+
        '</li>'+
        
        '<li class="kt-menu__item  kt-menu__item--submenu" aria-haspopup="true" data-ktmenu-submenu-toggle="hover"><a href="javascript:;" class="kt-menu__link kt-menu__toggle"><i class="kt-menu__link-icon flaticon-users"></i><span class="kt-menu__link-text">Utilizadores</span><i class="kt-menu__ver-arrow la la-angle-right"></i></a>'+
            '<div class="kt-menu__submenu "><span class="kt-menu__arrow"></span>'+
                '<ul class="kt-menu__subnav">'+
                    '<li class="kt-menu__item  kt-menu__item--parent" aria-haspopup="true"><span class="kt-menu__link"><span class="kt-menu__link-text">Utilizadores</span></span></li>'+
                    '<li class="kt-menu__item  kt-menu__item--submenu" aria-haspopup="true" data-ktmenu-submenu-toggle="hover"><a href="javascript:;" class="kt-menu__link kt-menu__toggle"><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Criar Novo</span><i class="kt-menu__ver-arrow la la-angle-right"></i></a>'+										
                    '</li>'+
                    '<li class="kt-menu__item  kt-menu__item--submenu" aria-haspopup="true" data-ktmenu-submenu-toggle="hover"><a href="javascript:;" class="kt-menu__link kt-menu__toggle"><i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i><span class="kt-menu__link-text">Lista</span><i class="kt-menu__ver-arrow la la-angle-right"></i></a>'+										
                    '</li>'+
                '</ul>'+
            '</div>'+
        '</li>'
    )
}





function AGILBSLatNavbarFooter(){

    $('.AGILBSLatNavbar .kt-aside__footer-nav.AGILBSNavbarMenuFooter').html(
        '<div class="kt-aside__footer-item">'+
            '<a href="#" class="btn btn-icon"><i class="flaticon2-gear"></i></a>'+
        '</div>'+
        '<div class="kt-aside__footer-item">'+
            '<a href="#" class="btn btn-icon"><i class="flaticon2-cube"></i></a>'+
        '</div>'+
        '<div class="kt-aside__footer-item">'+
            '<a href="#" class="btn btn-icon"><i class="flaticon2-bell-alarm-symbol"></i></a>'+
        '</div>'+
        '<div class="kt-aside__footer-item">'+
            '<button type="button" class="btn btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">'+
                '<i class="flaticon2-add"></i>'+
            '</button>'+
            '<div class="dropdown-menu dropdown-menu-left">'+
                '<ul class="kt-nav">'+
                    '<li class="kt-nav__section kt-nav__section--first">'+
                        '<span class="kt-nav__section-text">Export Tools</span>'+
                    '</li>'+
                    '<li class="kt-nav__item">'+
                        '<a href="#" class="kt-nav__link">'+
                            '<i class="kt-nav__link-icon la la-print"></i>'+
                            '<span class="kt-nav__link-text">Print</span>'+
                        '</a>'+
                    '</li>'+
                    '<li class="kt-nav__item">'+
                        '<a href="#" class="kt-nav__link">'+
                            '<i class="kt-nav__link-icon la la-copy"></i>'+
                            '<span class="kt-nav__link-text">Copy</span>'+
                        '</a>'+
                    '</li>'+
                    '<li class="kt-nav__item">'+
                        '<a href="#" class="kt-nav__link">'+
                            '<i class="kt-nav__link-icon la la-file-excel-o"></i>'+
                            '<span class="kt-nav__link-text">Excel</span>'+
                        '</a>'+
                    '</li>'+
                    '<li class="kt-nav__item">'+
                        '<a href="#" class="kt-nav__link">'+
                            '<i class="kt-nav__link-icon la la-file-text-o"></i>'+
                            '<span class="kt-nav__link-text">CSV</span>'+
                        '</a>'+
                    '</li>'+
                    '<li class="kt-nav__item">'+
                        '<a href="#" class="kt-nav__link">'+
                            '<i class="kt-nav__link-icon la la-file-pdf-o"></i>'+
                            '<span class="kt-nav__link-text">PDF</span>'+
                        '</a>'+
                    '</li>'+
                '</ul>'+
            '</div>'+
        '</div>'+
        '<div class="kt-aside__footer-item">'+
            '<a href="#" class="btn btn-icon"><i class="flaticon2-calendar-2"></i></a>'+
        '</div>'
    )

}






// Navbar Superior
function AGILBSSupNavbar(){


    $('.kt-header.AGILBSSupNavbar').html(

        // Menu Empresas a que têm acesso
        '<button class="kt-header-menu-wrapper-close" id="kt_header_menu_mobile_close_btn"><i class="la la-close"></i></button>'+
        '<div class="kt-header-menu-wrapper" id="kt_header_menu_wrapper">'+
            '<div id="kt_header_menu" class="kt-header-menu kt-header-menu-mobile  kt-header-menu--layout- ">'+
                '<ul class="kt-menu__nav ">'+
                    '<li class="kt-menu__item  kt-menu__item--submenu kt-menu__item--rel kt-menu__item--active" data-ktmenu-submenu-toggle="click" aria-haspopup="true">'+
                        '<a href="javascript:;" class="kt-menu__link kt-menu__toggle">'+
                            '<span class="kt-menu__link-icon"><i class="fa fa-briefcase"></i></span>'+											
                            '<span class="kt-menu__link-text">Adega Cooperativa de Vidigueira (ACV)</span>'+
                            '<i class="kt-menu__hor-arrow la la-angle-down"></i>'+
                            '<i class="kt-menu__ver-arrow la la-angle-right"></i>'+
                        '</a>'+
                        '<div class="kt-menu__submenu kt-menu__submenu--fixed kt-menu__submenu--left">'+
                            '<ul class="kt-menu__subnav">'+
                                
                                '<li class="kt-menu__item " aria-haspopup="true">'+
                                    '<a href="#" class="kt-menu__link ">'+
                                        '<i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i>'+
                                        '<span class="kt-menu__link-text">Empresa de testes Agildemo</span>'+
                                        '<span class="kt-menu__link-badge">AGILDEMO<!--span class="kt-badge kt-badge--success">2</span--></span>'+
                                    '</a>'+
                                '</li>'+

                                '<li class="kt-menu__item " aria-haspopup="true">'+
                                    '<a href="#" class="kt-menu__link ">'+
                                        '<i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i>'+
                                        '<span class="kt-menu__link-text">Encostas de Alqueva</span>'+
                                        '<span class="kt-menu__link-badge">001<!--span class="kt-badge kt-badge--success">2</span--></span>'+
                                    '</a>'+
                                '</li>'+

                                '<li class="kt-menu__item " aria-haspopup="true">'+
                                    '<a href="#" class="kt-menu__link ">'+
                                        '<i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i>'+
                                        '<span class="kt-menu__link-text">Adega Cooperativa de Borba</span>'+
                                        '<span class="kt-menu__link-badge">ACB<!--span class="kt-badge kt-badge--success">2</span--></span>'+
                                    '</a>'+
                                '</li>'+

                                '<li class="kt-menu__item " aria-haspopup="true">'+
                                    '<a href="#" class="kt-menu__link ">'+
                                        '<i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i>'+
                                        '<span class="kt-menu__link-text">Adega Cooperativa de Cantanhede</span>'+
                                        '<span class="kt-menu__link-badge">ACC<!--span class="kt-badge kt-badge--success">2</span--></span>'+
                                    '</a>'+
                                '</li>'+

                                '<li class="kt-menu__item " aria-haspopup="true">'+
                                    '<a href="#" class="kt-menu__link ">'+
                                        '<i class="kt-menu__link-bullet kt-menu__link-bullet--dot"><span></span></i>'+
                                        '<span class="kt-menu__link-text">Grupo AFSILVA</span>'+
                                        '<span class="kt-menu__link-badge">VG_Shared<!--span class="kt-badge kt-badge--success">2</span--></span>'+
                                    '</a>'+
                                '</li>'+

                            '</ul>'+
                        '</div>'+
                    '</li>'+
                    
                '</ul>'+
            '</div>'+
        '</div>'+
        // ACABA Menu Empresas a que têm acesso


        // CONTEUDO DE DADOS DO LOGIN E BARRA DE PESQUISA
		'<div class="kt-header__topbar">'+
			
        
            // Barra de Pesquisa
            '<div class="kt-header__topbar-item kt-header__topbar-item--search dropdown">'+
                '<div class="kt-header__topbar-wrapper" data-toggle="dropdown" data-offset="10px,0px">'+
                    '<span class="kt-header__topbar-icon"><i class="flaticon2-search-1"></i></span>'+
                '</div>'+
                '<div class="dropdown-menu dropdown-menu-fit dropdown-menu-right dropdown-menu-top-unround dropdown-menu-anim dropdown-menu-lg">'+
                    '<div class="kt-quick-search kt-quick-search--dropdown kt-quick-search--result-compact" id="kt_quick_search_dropdown">'+
                        '<form method="get" class="kt-quick-search__form">'+
                            '<div class="input-group">'+
                                '<div class="input-group-prepend"><span class="input-group-text"><i class="flaticon2-search-1"></i></span></div>'+
                                '<input type="text" class="form-control kt-quick-search__input" placeholder="Pesquisar...">'+
                                '<div class="input-group-append"><span class="input-group-text"><i class="la la-close kt-quick-search__close"></i></span></div>'+
                            '</div>'+
                        '</form>'+
                        '<div class="kt-quick-search__wrapper kt-scroll" data-scroll="true" data-height="325" data-mobile-height="200">'+
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>'+
            // Acaba Barra de Pesquisa

            


            
            
            // Dados do utilizador
            '<div class="kt-header__topbar-item kt-header__topbar-item--user">'+
                '<div class="kt-header__topbar-wrapper" data-toggle="dropdown" data-offset="0px,0px">'+

                    // <!--use "kt-rounded" class for rounded avatar style-->
                    '<div class="kt-header__topbar-user kt-rounded-">'+
                        '<span class="kt-header__topbar-welcome kt-hidden-mobile">Olá,</span>'+
                        '<span class="kt-header__topbar-username kt-hidden-mobile">Fernando</span>'+
                        '<img alt="Pic" src="assets/media/users/300_25.jpg" class="kt-rounded-" />'+

                        //<!--use below badge element instead the user avatar to display username's first letter(remove kt-hidden class to display it) -->
                        '<span class="kt-badge kt-badge--username kt-badge--lg kt-badge--brand kt-hidden kt-badge--bold">S</span>'+
                    '</div>'+
                '</div>'+
                '<div class="dropdown-menu dropdown-menu-fit dropdown-menu-right dropdown-menu-anim dropdown-menu-top-unround dropdown-menu-sm">'+
                    '<div class="kt-user-card kt-margin-b-40 kt-margin-b-30-tablet-and-mobile" style="background-image: url(assets/media/misc/head_bg_sm.jpg)">'+
                        '<div class="kt-user-card__wrapper">'+
                            '<div class="kt-user-card__pic">'+

                                //<!--use "kt-rounded" class for rounded avatar style-->
                                '<img alt="Pic" src="assets/media/users/300_21.jpg" class="kt-rounded-" />'+
                            '</div>'+
                            '<div class="kt-user-card__details">'+
                                '<div class="kt-user-card__name">Fernando Carrujo</div>'+
                                '<div class="kt-user-card__position">Vendedor</div>'+
                            '</div>'+
                        '</div>'+
                    '</div>'+
                    '<ul class="kt-nav kt-margin-b-10">'+
                        '<li class="kt-nav__item">'+
                            '<a href="custom/profile/personal-information.html" class="kt-nav__link">'+
                                '<span class="kt-nav__link-icon"><i class="flaticon2-calendar-3"></i></span>'+
                                '<span class="kt-nav__link-text">Meus Dados</span>'+
                            '</a>'+
                        '</li>'+
                        
                        '<li class="kt-nav__item">'+
                            '<a href="custom/profile/account-settings.html" class="kt-nav__link">'+
                                '<span class="kt-nav__link-icon"><i class="flaticon2-gear"></i></span>'+
                                '<span class="kt-nav__link-text">Configurações</span>'+
                            '</a>'+
                        '</li>'+
                        '<li class="kt-nav__separator kt-nav__separator--fit"></li>'+
                        '<li class="kt-nav__custom kt-space-between">'+
                            '<a href="custom/login/login-v1.html" target="_blank" class="btn btn-label-brand btn-upper btn-sm btn-bold">Terminar Sessão</a>'+
                            '<i class="flaticon2-information kt-label-font-color-2" data-toggle="kt-tooltip" data-placement="right" title="" data-original-title="Click to learn more..."></i>'+
                        '</li>'+
                    '</ul>'+
                '</div>'+
            '</div>'+
            // ACABAM DADOS DO UTILIZADOR
            


            //'</div>'

        
        '</div>'
        // ACABA CONTEUDO DE DADOS DO LOGIN E BARRA DE PESQUISA

    );

}
AGILBSSupNavbar();











// RODAPÉ DA PLATAFORMA
function AGILBSRodape(){

    $('.kt-footer.AGILBSRodape').html(
        '<div class="kt-container  kt-container--fluid ">'+
            '<div class="kt-footer__copyright">'+
                '2020&nbsp;&copy;&nbsp;<a href="http://www.parsisplan.pt" target="_blank" class="kt-link">AGILBS | Parsisplan SI</a>'+
            '</div>'+
            '<div class="kt-footer__menu">'+
                '<a href="http://www.parsisplan.pt/contactos/" target="_blank" class="kt-footer__menu-link kt-link">Contactos</a>'+
            '</div>'+
        '</div>'
    )

}
AGILBSRodape();




