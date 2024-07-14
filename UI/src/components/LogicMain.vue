<template>
    <main-template>
        <div class="logic-main">
            <div class="tabs">
                <div class="tab" @click="selectTab('main')" :class="{'nav-active': activeTab == 'main'}">Processes</div>
                <div class="tab" v-for="(tab, index) in tabs" :key="tab" @click="selectTab(tab.id)" :class="{'nav-active': activeTab == tab.id}">
                    {{ tab.name }}
                    <div @click.stop="closeTab(index, tab.id)">
                        <span class="material-symbols-outlined close" style="color: red;">close</span>
                    </div>
                </div>
            </div>
            <div class="logic-main-content" @click="showSideBar = false">
                <div class="logic-main-workspace">
                    <process-list class="content-animation" v-show="activeTab == 'main'" />
                    <logic-workflow class="content-animation" v-for="tab in tabs" :key="tab" v-show="activeTab == tab.id" :id="tab.id" />
                </div>
            </div>
        </div>
    </main-template>
</template>

<script setup>
import LogicWorkflow from "./LogicPage/LogicWorkflow.vue"
import MainTemplate from "./MainTemplate.vue"
import ProcessList from "./LogicPage/ProcessList.vue"
import { selectTab, closeTab } from "./scripts/functions"
import store from "./store"

const { tabs, activeTab } = store();
</script>

<style scoped>
.logic-main {
    position: relative;
    display: flex;
    flex-direction: column;
    height: calc(100vh - 50px);
    width: 100%;
    padding: 0;
}

.logic-main .logic-main-content {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 100%;
    padding: 0;
    border-top: 1px solid rgb(217, 212, 212);
}

.logic-main .logic-main-content .logic-main-workspace {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
}
.tabs {
    height: 35px;
    width: 100%;
    margin-top: 15px;
    display: flex;
}
.tabs * {
    font-size: 10px;
}
.tabs .tab {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    border: 1px solid rgb(224, 224, 224);
    padding: 10px;
    transition: 0.5s;
    border: 1px solid grey;
    border-bottom: none;
    cursor: pointer;
    background: #e0e0e0;
    border-top-right-radius: 5px;
    border-top-left-radius: 5px;
}
.tabs .tab:hover {
    filter: brightness(1.1);
}
.tabs .tab.nav-active {
    background: #3990C5;
    color: #fff;
}
.close {
    margin-left: 5px;
}
.content-animation {
    animation: 0.5s fade;
}
.tab-container {
    position: absolute;
    display: flex;
    height: 100%;
    width: 100%;
}
</style>