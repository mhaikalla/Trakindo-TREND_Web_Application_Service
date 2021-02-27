/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'links', groups: ['links'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'forms', groups: ['forms'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'others', groups: ['others'] },
        '/',
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'styles', groups: ['styles'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'about', groups: ['about'] }
    ];

    //config.removeButtons = 'Underline,Subscript,Superscript,HorizontalRule,SpecialChar,Anchor,Scayt,Strike,Cut,Undo,Redo,Copy,Paste,PasteText,PasteFromWord,Blockquote,Styles';
    
    config.extraPlugins = 'uploadimage, justify, font';
    //config.uploadUrl = '';

      // Configure your file manager integration. This example uses CKFinder 3 for PHP.
    //config.filebrowserBrowseUrl = '';
    //config.filebrowserImageBrowseUrl = '';
    //config.filebrowserUploadUrl = '';
    config.filebrowserImageUploadUrl = window.location.origin + '/api/article/uploadimagecontent';
    //config.filebrowserImageUploadUrl = window.location.origin + '/trend/api/article/uploadimagecontent';
    config.height = 400;

    config.removeDialogTabs = 'image:advanced;link:advanced';
};
