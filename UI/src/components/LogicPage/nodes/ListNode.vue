<template>
    <node-template type="list" :id="id">
        <div class="input">
            <label for="type">Type</label>
            <select id="type" v-model="node.data.type">
                <option value="string">String</option>
                <option value="object">Object</option>
            </select>
        </div>
        <div class="input">
            <label>List</label>
            <div class="list" v-for="(item, index) in node.data.list" :key="item">
                <span class="material-symbols-outlined delete-btn" @click="removeToList(index)">delete</span>
                <NodeTextarea :node="node" rows="4" cols="30" v-if="node.data.type == 'object'" v-model="item.value"></NodeTextarea> 
                <NodeTextbox :node="node" v-else v-model="item.value"></NodeTextbox>
            </div>
            <button class="add-list">
                <span class="material-symbols-outlined" @click="addToList">add</span>
            </button>
        </div>
        <div class="input">
            <label for="variable">Variable</label>
            <VTextbox :node="node" id="variable" v-model="node.data.variable"></VTextbox>
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"
import NodeTextarea from "../components/NodeTextarea.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
const addToList = function () {
    node.data.list.push({
        value: ""
    }); 
}
const removeToList = function (index) {
    node.data.list.splice(index, 1);
}
</script>

<style scoped>
@import "./inputStyle.css";
.add-list {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    border: none;
    outline: none;
    border-radius: 90px;
    margin-top: 10px;
    cursor: pointer;
    transition: 0.3s;
}
.add-list:hover {
    background: #87B359;
    color: #fff;
    box-shadow: 0 0 10px #87B359;
}
.add-list span {
    font-size: 13px;
}
.list {
    position: relative;
    display: flex;
    overflow: auto;
    text-wrap: nowrap;
    margin-top: 5px;
}
.delete-btn {
    display: flex;
    justify-content: center;
    align-items: center;
    width: fit-content !important;
    font-size: 15px;
    cursor: pointer;
    transition: 0.3s;
}
.delete-btn:hover {
    color: red;
}
</style>