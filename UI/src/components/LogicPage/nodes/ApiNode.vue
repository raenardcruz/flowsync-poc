<template>
    <node-template type="Rest API" :id="id">
        <div class="input">
            <label for="url">Url</label>
            <NodeTextbox id="url" :node="node" class="textbox" v-model="node.data.url" placeholder="Enter the URL" />
        </div>
        <div class="input">
            <label for="method">Method</label>
            <select id="method" v-model="node.data.method">
                <option value="GET">Get</option>
                <option value="POST">Post</option>
                <option value="Put">Put</option>
                <option value="Patch">Patch</option>
                <option value="Delete">Delete</option>
            </select>
        </div>
        <div class="input">
            <label for="headers">Headers</label>
            <div class="header-container table">
                <div class="row">
                    <div class="column header">
                        Key
                    </div>
                    <div class="column header">
                        Value
                    </div>
                </div>
                <div class="row">
                    <div class="column">
                        <input class="table-input" type="text" value="Content-type" disabled>
                    </div>
                    <div class="column">
                        <select v-model="node.data.headers[0].value" class="table-input" @change="updatePayload()">
                                <option value="text/plain">text/plain</option>
                                <option value="text/html">text/html</option>
                                <option value="text/css">text/css</option>
                                <option value="text/javascript">text/javascript</option>
                                <option value="application/xml">application/xml</option>
                                <option value="application/json">application/json</option>
                                <option value="application/x-www-form-urlencoded">application/x-www-form-urlencoded
                                </option>
                                <option value="multipart/form-data">multipart/form-data</option>
                                <option value="application/octet-stream">application/octet-stream</option>
                                <option value="application/pdf">application/pdf</option>
                                <option value="image/jpeg">image/jpeg</option>
                                <option value="image/png">image/png</option>
                                <option value="image/gif">image/gif</option>
                                <option value="image/webp">image/webp</option>
                                <option value="audio/mpeg">audio/mpeg</option>
                                <option value="audio/ogg">audio/ogg</option>
                                <option value="video/mp4">video/mp4</option>
                                <option value="video/webm">video/webm</option>
                                <option value="application/zip">application/zip</option>
                            </select>
                        </div>
                </div>
                <div
                class="row"
                v-for="(header, index) in node.data.headers.filter(f => f.key != 'Content-Type')"
                :key="header">
                    <div class="column">
                        <div class="delete-btn" @click="deleteheader(index + 1)">
                            <span class="material-symbols-outlined">delete</span>
                        </div>
                        <NodeTextbox id="url" :node="node" class="table-input" type="text" v-model="header.key" />
                    </div>
                    <div class="column">
                        <NodeTextbox id="url" :node="node" class="table-input" type="text" v-model="header.value" />
                    </div>
                </div>
                <div class="row">
                    <button class="table-add-btn" @click="addHeader">
                        <span class="material-symbols-outlined">add</span>
                    </button>
                </div>
            </div>
        </div>
        <div class="input" v-if="node.data.method != 'GET' && node.data.method != 'DELETE'">
            <label for="payload">Payload</label>
            <div class="table" v-if="node.data.headers[0].value == 'application/x-www-form-urlencoded' || node.data.headers[0].value == 'multipart/form-data'">
                <div class="row">
                    <div class="column header">
                        Key
                    </div>
                    <div class="column header">
                        Value
                    </div>
                </div>
                <div class="row" v-for="(header, index) in node.data.payload" :key="header">
                    <div class="column">
                        <div class="delete-btn" @click="deletepayload(index)">
                            <span class="material-symbols-outlined">delete</span>
                        </div>
                        <NodeTextbox id="url" :node="node" class="table-input" type="text" v-model="header.key" />
                    </div>
                    <div class="column">
                        <NodeTextbox id="url" :node="node" class="table-input" type="text" v-model="header.value" />
                    </div>
                </div>
                <div class="row">
                    <button class="table-add-btn"  @click="addPayload">
                        <span class="material-symbols-outlined">add</span>
                    </button>
                </div>
            </div>
            <NodeTextarea :node="node" v-model="node.data.payload" rows="4" cols="50" v-else></NodeTextarea>
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
import { computed } from "vue";
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"
import NodeTextarea from "../components/NodeTextarea.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
const headers = computed(() => node.data.headers[0].value);

const addHeader = function () {
    node.data.headers.push({
        key: "",
        value: ""
    })
}
const addPayload = function () {
    if (node.data.payload.length == 0)
        node.data.payload = [];    
    node.data.payload.push({
        key: "",
        value: ""
    })
}
const deleteheader = function (index) {
    node.data.headers.splice(index, 1)
}
const deletepayload = function (index) {
    node.data.payload.splice(index, 1)
}
const updatePayload = function () {
    if (headers.value == "multipart/form-data" || headers.value == "application/x-www-form-urlencoded")
        node.data.payload = [];
    else
        node.data.payload = "";
}
</script>

<style scoped>
@import "./inputStyle.css";

.table {
    position: relative;
    display: flex;
    flex-direction: column;
    width: 100%;
    height: fit-content;
    justify-content: center;
    align-items: center;
}

.row {
    position: relative;
    display: flex;
    gap: 2px;
    width: 100%;
    padding: 3px;
}

.column {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    font-size: 10px;
    min-height: 10px;
    height: fit-content;
}
.delete-btn {
    width: 20px !important;
    transition: 0.3s linear;
}
.delete-btn:hover {
    color: red;
}
.table-add-btn {
    position: relative;
    display: flex;
    height: 17px;
    justify-content: center;
    align-items: center;
    transition: 0.2s linear;
    border-radius: 20px;
    cursor: pointer;
    margin-top: 10px;
    color: green;
    border: none;
    background: #f0f0f0;
    box-shadow:  3px 3px 5px #868686,
             -3px -3px 5px #ffffff;
}
.table-add-btn:hover {
    background: #f0f0f0;
    box-shadow:  inset 3px 3px 5px #868686,
        inset -3px -3px 5px #ffffff;
}
.header {
    font-weight: 600;
    background: grey;
    color: #fff;
}

.material-symbols-outlined {
    font-size: 13px;
}
</style>