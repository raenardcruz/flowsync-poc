<template>
    <div class="logic-main-sidebar" :class="showSideBar ? 'show' : ''">
        <div class="toggle-sidebar" @click="showSideBar = !showSideBar">
            <span class="material-symbols-outlined" v-if="showSideBar">keyboard_double_arrow_left</span>
            <span class="material-symbols-outlined" v-else>keyboard_double_arrow_right</span>
        </div>
        <div class="sidebar">
            <div class="input">
                <input type="search" placeholder="Search Nodes" v-model="search">
            </div>
            <div
            class="node"
                v-for="node in searchNode"
                :key="node"
                :draggable="true"
                @dragstart="onDragStart(node)"
                v-if="search.length > 0">
                <div class="node-icon" :style="'color:' + iconColor(node.type).color">
                    <span class="material-symbols-outlined">{{ iconColor(node.type).icon }}</span>
                </div>
                <div class="node-type">
                    {{ node.type }}
                </div>
            </div>
            <div class="group" @click="expandToggle('all')"  v-if="search.length == 0">
                <div class="name">
                    <span class="material-symbols-outlined grp-icon">apps</span>
                    <span>All</span>
                    <span class="material-symbols-outlined expand-icon" :class="{'expand': expandGroup.includes('all')}">arrow_forward_ios</span>
                </div>
                <div class="description">Contains all the nodes</div>
                <div class="nodes" :class="{'expand': expandGroup.includes('all')}">
                    <div class="node" v-for="node in nodeTemplates" :key="node" :draggable="true" @dragstart="onDragStart(node)" @click.stop>
                        <div class="node-icon" :style="'color:' + iconColor(node.type).color">
                            <span class="material-symbols-outlined">{{ iconColor(node.type).icon }}</span>
                        </div>
                        <div class="node-type">
                            {{ node.type }}
                        </div>
                    </div>
                </div>
            </div>
            <div
                class="group" 
                v-for="group in nodeGroups" 
                :key="group.name"
                @click="expandToggle(group.name)"
                v-if="search.length == 0">
                <div class="name">
                    <span class="material-symbols-outlined grp-icon">{{ group.icon }}</span>
                    <span>{{ group.name }}</span>
                    <span class="material-symbols-outlined expand-icon" :class="{'expand': expandGroup.includes(group.name)}">arrow_forward_ios</span>
                </div>
                <div class="description">{{ group.description }}</div>
                <div class="nodes" :class="{'expand': expandGroup.includes(group.name)}">
                    <div class="node" v-for="node in groupNodes(group.name)" :key="node" :draggable="true" @dragstart="onDragStart(node)" @click.stop>
                        <div class="node-icon" :style="'color:' + iconColor(node.type).color">
                            <span class="material-symbols-outlined">{{ iconColor(node.type).icon }}</span>
                        </div>
                        <div class="node-type">
                            {{ node.type }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed } from "vue"
import store from "../../store.js";
import { nodeTemplates, iconStyles, nodeGroups } from "../scripts/constants.js";

const { draggedNode, showSideBar } = store();

const expandGroup = ref([])
const search = ref('')

const expandToggle = function (name) {
    if (expandGroup.value.includes(name))
        expandGroup.value.splice(expandGroup.value.findIndex(f => f == name));
    else
        expandGroup.value.push(name)
}

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
const searchNode = computed(() => {
    return nodeTemplates.filter(f => f.type.toLowerCase().includes(search.value))
})
const groupNodes = function(groupName) {
    var tmpNodes = [];
    var group = nodeGroups.find(f => f.name == groupName);
    for (var type of group.members) {
        var filteredNode = nodeTemplates.filter(f => f.type == type);
        if (filteredNode.length > 0)
            tmpNodes.push(filteredNode[0])
    }
    return tmpNodes;
}
</script>

<style scoped>
@import "../nodes/inputStyle.css";
.logic-main .logic-main-sidebar {
    position: absolute;
    display: flex;
    flex-direction: column;
    justify-self: left;
    height: 100%;
    width: 280px;
    z-index: 2;
    transition: 0.3s ease-in-out;
    left: -280px;
}

.show {
    left: 0px !important;
    box-shadow: 4px 2px 4px grey;
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
    transition: 0.3s ease-in-out;
    background: #f0f0f0;
    overflow: auto;
    padding: 20px;
}
.group {
    position: relative;
    display: flex;
    border-top: solid 1px grey;
    padding: 10px;
    justify-content: flex-start;
    align-items: center;
    flex-wrap: wrap;
    flex-direction: column;
    cursor: pointer;
    transition: 0.3s ease-in-out;
}
.group:hover .name {
    color: rgb(184, 106, 17);
}
.group .name {
    font-size: 15px;
    font-weight: 600;
    color: rgb(40, 39, 39);
    transition: 0.3s ease-in-out;
}

.group .description {
    font-size: 10px;
    color: rgb(72, 72, 72)
}
.group .nodes {
    position: relative;
    display: flex;
    flex-direction: column;
    width: 100%;
    transition: 0.5s ease-out;
    max-height: 0;
    opacity: 0;
}
.group .nodes.expand {
    max-height: 1000vh; /* Set this to a value that is enough to show your content */
    opacity: 1;
}
.node {
    position: relative;
    display: flex;
    border: 0.5px solid #B7B8B8;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
    border-radius: 90px;
    height: 30px;
    cursor: grab;
    transition: 0.3s;
    font-size: 10px;
    background: linear-gradient(145deg, #ffffff, #d8d8d8);
}

.node:hover {
    background: linear-gradient(145deg, #d8d8d8, #ffffff);
    box-shadow: 4px 4px 5px #868686,
        -4px -4px 5px #ffffff;
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
.grp-icon {
    font-size: 13px;
    margin-right: 10px;
}
.expand-icon {
    font-size: 10px;
    margin-left: 10px;
    transition: 0.3s ease-in-out;
}
.expand-icon.expand {
    transform: rotate(90deg);
}
</style>