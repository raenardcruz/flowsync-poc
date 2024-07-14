<template>
    <div class="logic-main-sidebar" :class="showSideBar ? 'show' : ''">
        <div class="toggle-sidebar" @click="showSideBar = !showSideBar">
            <span class="material-symbols-outlined" v-if="showSideBar">keyboard_double_arrow_left</span>
            <span class="material-symbols-outlined" v-else>keyboard_double_arrow_right</span>
        </div>
        <div class="sidebar">
            <div class="title">Nodes</div>
            <div class="node" v-for="node in nodeTemplates" :key="node" :draggable="true"
                @dragstart="onDragStart(node)">
                <div class="node-icon" :style="'color:' + iconColor(node.type).color">
                    <span class="material-symbols-outlined">{{ iconColor(node.type).icon }}</span>
                </div>
                <div class="node-type">
                    {{ node.type }}
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import store from "../../store.js";
import { nodeTemplates, iconStyles } from "../scripts/constants.js";

const { draggedNode, showSideBar } = store();

const iconColor = function (type) {
    var filterStyle = iconStyles.filter(f => f.name == type);
    if (filterStyle.length > 0)
        return filterStyle[0];
    else
        return {
            icon: "square",
            color: "#fff"
        }
}

const onDragStart = function (node) {
    draggedNode.value = node;
    document.addEventListener('drop', onDragEnd);
}
const onDragEnd = function () {
    draggedNode.value = null
    document.removeEventListener('drop', onDragEnd)
}
</script>

<style scoped>
.logic-main .logic-main-sidebar {
    position: absolute;
    display: flex;
    flex-direction: column;
    justify-self: left;
    height: 100%;
    width: 280px;
    z-index: 2;
    transition: 0.3s;
    left: -280px;
}

.show {
    left: 0px !important;
}

.logic-main .logic-main-sidebar .toggle-sidebar {
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 50%;
    right: -40px;
    height: 30px;
    width: 30px;
    background: #fff;
    border-radius: 90px;
    cursor: pointer;
    box-shadow: 0px 5px 10px grey;
}

.sidebar {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-self: left;
    height: 100%;
    width: 280px;
    z-index: 2;
    transition: 0.3s;
    background: #f0f0f0;
    overflow: auto;
    padding: 20px;
}

.node {
    position: relative;
    display: flex;
    border: 0.5px solid #B7B8B8;
    margin-top: 20px;
    border-radius: 90px;
    height: 50px;
    cursor: grab;
    transition: 0.3s;
    background: linear-gradient(145deg, #ffffff, #d8d8d8);
}

.node:hover {
    background: linear-gradient(145deg, #d8d8d8, #ffffff);
    box-shadow: 10px 10px 20px #868686,
        -10px -10px 20px #ffffff;
    transform: scale(1.02);
}

.node .node-icon {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 30%;
}

.node .node-type {
    position: relative;
    display: flex;
    height: 100%;
    align-items: center;
    width: 70%;
    font-size: 15px;
    padding: 20px;
}
</style>