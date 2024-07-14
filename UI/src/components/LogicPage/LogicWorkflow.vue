<template>
  <div class="tab-container">
    <div class="title-section">
      <div class="title">
        <input type="text" v-model="tab.name" placeholder="Enter Process Title">
      </div>
      <div class="description">
        <input type="text" v-model="tab.description" placeholder="Enter Description">
      </div>
    </div>
    <div class="content">
      <log-modal :id="id" v-if="tab.showLogModal" />
      <logic-side-bar />
      <Notif ref="notify" />
      <confirmation-modal ref="confirmation" />
      <VueFlow :class="id" class="basicflow" :nodes="tab.nodes" :edges="tab.edges" :node-types="nodeTypes" :edge-types="edgeTypes"
        :default-viewport="{ zoom: 1 }" :min-zoom="0.1" :max-zoom="5" :only-render-visible-elements="false"
        @nodes-change="onChangeNodes" @edges-change="onChangeEdge" @dragover="onDragOver" @drop="onDrop"
        no-wheel-class-name="no-scroll" @mousemove="handleMouseMove" delete-key-code="false" @keydown="handleKeyDown"
        tabindex="0">
        <Background pattern-color="#ABABAB" :size="1" :gap="20" />
        <Controls :showInteractive="false" position="top-right">
          <ControlButton title="Reset Transform" @click="resetTransform">
            <span class="material-symbols-outlined">refresh</span>
          </ControlButton>
          <ControlButton title="Delete" @click="deleteProcess(tab, confirmation, notify)" v-if="!tab.runMode && !tab.isNew">
            <span class="material-symbols-outlined">delete</span>
          </ControlButton>
          <ControlButton title="Save" @click="save(tab, notify)" v-if="!tab.runMode">
            <span class="material-symbols-outlined">save</span>
          </ControlButton>
          <ControlButton title="Run" style="background: #6FA071; color: #fff;" @click="quickrun(tab, notify)" v-if="!tab.runMode">
            <span class="material-symbols-outlined">play_arrow</span>
          </ControlButton>
        </Controls>
      </VueFlow>
      <div class="run-btns" v-if="tab.runMode">
        <div class="log-btn" @click="tab.showLogModal = true" v-if="tab.runComplete">
          <span class="material-symbols-outlined" style="color: #0088C2; margin-right: 10px">description</span>
          <span>Show Logs</span>
        </div>
        <div class="close-btn" @click="exitRunMode(tab)">
          <span class="material-symbols-outlined" style="color: #BC0F26; margin-right: 10px">cancel</span>
          <span>Exit Run Mode</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
/*
Section: Import
Description: This section defines the imports that this page will be using 
*/
import LogicSideBar from "./components/LogicSideBar.vue"
import { VueFlow, useVueFlow } from '@vue-flow/core'
import { Background } from '@vue-flow/background'
import { ControlButton, Controls } from '@vue-flow/controls'
import { nodeTypes, edgeTypes } from "./scripts/constants.js"
import { ref } from "vue";
import LogModal from "./components/LogModal.vue"
import Notif from "../Common/Notif.vue"
import ConfirmationModal from "../Common/ConfirmationModal.vue"
import SignalREvents from "./scripts/signalREvents.js"
import VueFlowEvents from "./scripts/vueFlowEvents.js"
import { 
  findTab, 
  save,
  deleteProcess,
  exitRunMode,
  quickrun 
} from "./scripts/functions.js"

const notify = ref(null);
const confirmation = ref(null);
const props = defineProps(['id']);

const tab = findTab(props.id);
var signalREvent = new SignalREvents(props.id);
signalREvent.initializeEvents();
var vueFlowEvent = new VueFlowEvents(props.id, useVueFlow());
vueFlowEvent.initializeEvents();

const resetTransform = vueFlowEvent.resetTransform;
const onChangeNodes = vueFlowEvent.onChangeNodes;
const onChangeEdge = vueFlowEvent.onChangeEdge;
const onDrop = vueFlowEvent.onDrop;
const handleMouseMove = vueFlowEvent.handleMouseMove;
const handleKeyDown = vueFlowEvent.handleKeyDown;

const onDragOver = (event) => event.preventDefault();
</script>

<style>
@import '@vue-flow/core/dist/style.css';
@import '@vue-flow/core/dist/theme-default.css';

.tab-container {
  position: relative;
  display: flex;
  flex-direction: column;
  height: 100%;
  width: 100%;
}

.title-section {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: start;
  align-items: start;
  height: fit-content;
  width: 100%;
  padding: 10px 0px 10px 20px;
  box-shadow: 0 5px 5px grey;
}

.title-section .title input {
  font-size: 30px;
  field-sizing: content;
}

.title-section .description input {
  font-size: 12px;
  color: grey;
  field-sizing: content;
}

.title-section input {
  border: none;
  background: none;
  padding: 5px;
  border-radius: 20px;
}

.title-section input:hover {
  border: 1px solid rgb(200, 192, 192);
}

.title-section input:focus-visible {
  outline-color: rgb(200, 192, 192);
}

.content {
  position: relative;
  display: flex;
  height: 100%;
  width: 100%;
  justify-content: center;
  align-items: end;
}

.my-custom-node-class {
  background: purple;
  color: white;
  border: 1px solid purple;
  border-radius: 4px;
  box-shadow: 0 0 0 1px purple;
  padding: 8px;
}

.my-custom-node-class:focus-visible {
  border-color: pink;
}

.active {
  color: #fff;
  background: #3489CD;
  box-shadow: 0 0 5px #3489CD;
  transition: 0.2s;
}

.active:hover {
  filter: brightness(120%);
  transform: scale(1.02);
  background: #3489CD;
}

.material-symbols-outlined {
  font-size: 18px;
}

.run-btns {
  position: absolute;
  top: 20px;
  left: 43%;
  display: flex;
  gap: 20px;
}

.run-btns .log-btn {
  position: relative;
  display: flex;
  height: 30px;
  width: fit-content;
  justify-content: center;
  align-items: center;
  border-radius: 90px;
  font-size: 13px;
  padding: 10px;
  cursor: pointer;
  background: #f0f0f0;
  transition: 0.3s;
  box-shadow: 6px 6px 12px #868686,
    -6px -6px 12px #ffffff;
  ;
}

.run-btns .close-btn {
  position: relative;
  display: flex;
  height: 30px;
  width: fit-content;
  justify-content: center;
  align-items: center;
  border-radius: 90px;
  font-size: 13px;
  padding: 10px;
  cursor: pointer;
  background: #f0f0f0;
  transition: 0.3s;
  box-shadow: 6px 6px 12px #868686,
    -6px -6px 12px #ffffff;
  ;
}

.run-btns .log-btn:hover,
.run-btns .close-btn:hover {
  transform: scale(1.02);
  box-shadow: 15px 15px 30px #868686,
    -15px -15px 30px #ffffff;
}
</style>