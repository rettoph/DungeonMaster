import Vue from 'vue';

const DungeonMasterVue =
{
    Init: function (element, componentName, componentObject) {
        document.addEventListener('DOMContentLoaded', function () {
            window.VueApp = new Vue({
                el: element,
                components: {
                    [componentName]: componentObject
                }
            });
        })
    }
}

export default DungeonMasterVue;