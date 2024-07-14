<template>
    <div class="options no-scroll" :class="{'show' : showOptions}">
        <div class="close" @click="showOptions = false">
            <span class="material-symbols-outlined" style="color: red;">close</span>
        </div>
        <div class="title">
            <span class="material-symbols-outlined">settings</span>
            Options
        </div>
        <div class="field-label">Start Type</div>
        <div class="selections">
            <div class="selection" @click="tab.type = 'default'" :class="{'selected': tab.type == 'default'}">
                <span class="material-symbols-outlined">flag_circle</span>
                <span class="hidden-title">Default</span>
            </div>
            <div class="selection" @click="tab.type = 'interval'" :class="{'selected': tab.type == 'interval'}">
                <span class="material-symbols-outlined">calendar_month</span>
                <span class="hidden-title">Interval</span>
            </div>
            <div class="selection" @click="tab.type = 'webhook'" :class="{'selected': tab.type == 'webhook'}">
                <span class="material-symbols-outlined">webhook</span>
                <span class="hidden-title">Webhook</span>
            </div>
            <div class="selection" @click="tab.type = 'events'" :class="{'selected': tab.type == 'events'}">
                <span class="material-symbols-outlined">event</span>
                <span class="hidden-title">Event</span>
            </div>
        </div>
        <div class="field-label">Settings</div>
        <div class="field" v-if="tab.type == 'webhook'">
            <label for="webhookname">Name</label>
            <input type="text" id="webhookname" class="input" v-model="tab.webhookName">
        </div>
        <div class="field" v-if="tab.type == 'webhook'">
            <label for="webhookUrl">Url</label>
            <input type="text" id="webhookUrl" class="input" v-model="webhookUrl" disabled>
        </div>
        <div class="field" v-if="tab.type == 'interval'">
            <label for="interval-type">Type</label>
            <select class="input"  id="interval-type" v-model="tab.intervalType">
                <option value="minute">Minute</option>
                <option value="hour">Hour</option>
                <option value="day">Day</option>
                <option value="month">Month</option>
            </select>
        </div>
        <div class="field" v-if="tab.type == 'interval'">
            <label for="interval-start-time">Start Time</label>
            <input  class="input"  type="time" name="myInput" v-model="tab.intervalStartTime">
        </div>
        <div class="field" v-if="tab.type == 'interval'">
            <label for="interval-end-time">End Time</label>
            <input  class="input"  type="time" name="myInput" v-model="tab.intervalEndTime">
        </div>
        <div class="field" v-if="tab.type == 'interval'">
            <label for="interval">Interval</label>
            <input type="number" id="interval" class="input" v-model="tab.interval">
        </div>
        <div class="field" v-if="tab.type == 'interval'">
            <label for="interval-end-time">Days of the week</label>
        </div>
        <div v-if="tab.type == 'interval'">
            <check-box v-model="tab.intervalDaysofWeek[0]" label="Sunday" />
            <check-box v-model="tab.intervalDaysofWeek[1]" label="Monday" />
            <check-box v-model="tab.intervalDaysofWeek[2]" label="Tuesday" />
            <check-box v-model="tab.intervalDaysofWeek[3]" label="Wednesday" />
            <check-box v-model="tab.intervalDaysofWeek[4]" label="Thursday" />
            <check-box v-model="tab.intervalDaysofWeek[5]" label="Friday" />
            <check-box v-model="tab.intervalDaysofWeek[6]" label="Saturday" />
        </div>
        <div class="field" v-if="tab.type == 'events'">
            <label for="eventname">Event Name</label>
    <input type="text" id="eventname" class="input" v-model="tab.eventName">
        </div>
    </div>
    <div class="start-node" @click="showOptions = !showOptions" :class="tab.type">
        <span class="material-symbols-outlined" v-if="tab.type == 'default'">flag_circle</span>
        <span class="material-symbols-outlined" v-else-if="tab.type == 'interval'">schedule</span>
        <span class="material-symbols-outlined" v-else-if="tab.type == 'webhook'">webhook</span>
        <span class="material-symbols-outlined" v-else-if="tab.type == 'events'">event</span>
        {{ tab.type }}
    </div>
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import CheckBox from "../../Common/CheckBox.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import store from "../../store"
import { ref, watch, computed } from "vue"

const { tabs } = store();
const showOptions = ref(false);
const { 
  node
} = useNode()

var tab = tabs.value.find(f => f.id == node.tabId);
const webhookUrl = computed(() => `https://localhost:7217/dev/api/${tab.webhookName}?id=${tab.id}`)

node.draggable = false;
</script>

<style scoped>
.start-node {
    width: fit-content;
    border-radius: 90px;
    color: #fff;
    display: flex;
    justify-content: center;
    font-size: 15px;
    align-items: center;
    cursor: pointer;
    transition: 0.3s;
    padding: 20px;
    text-transform: uppercase;
    box-shadow:  7px 7px 14px #7d7d7d,
             -7px -7px 14px #ffffff;
}
.start-node:hover {
    transform: scale(1.1);
    filter: brightness(1.1);
}
.options {
    position: absolute;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    max-height: 300px;
    overflow: auto;
    top: -50%;
    left: 50%;
    height: 0px;
    width: 0px;
    opacity: 0;
    border-radius: 8px;
    transition: 0.2s;
    padding: 10px;
    background: #f0f0f0;
    box-shadow:  6px 6px 12px #868686,
             -6px -6px 12px #ffffff;
}
.show {
    height: fit-content;
    width: 250px;
    opacity: 1;
    left: 110%;
    top: -110%;
}
.options .title {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    font-size: 15px;
    font-weight: 500;
    margin-bottom: 10px;
}
.field-label {
    font-size: 12px;
}
.selections {
    position: relative;
    display: flex;
    flex-wrap: wrap;
    width: 100%;
    align-content: center;
    justify-content: center;
    padding: 15px;
    gap: 15px;
}
.selections .selection {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
    padding: 5px 10px 5px 10px;
    border-radius: 8px;
    background: #f0f0f0;
    box-shadow: 5px 5px 9px #5a5a5a,
        -5px -5px 9px #ffffff;
}
.selections .selection.selected {
    box-shadow: inset 5px 5px 9px #5a5a5a,
        inset -5px -5px 9px #ffffff;
}
.selections .selection span {
    font-size: 10px;    
}
.default {
    background: #84ab86;
}
.interval {
    background: #09a6d6;
}
.webhook {
    background: #b86a11;
}
.events {
    background: #A3245B;
}
.material-symbols-outlined {
    margin-right: 10px;
}
.field {
    position: relative;
    display: flex;
    padding: 10px;
    border: none;
    width: 100%;
}
.field label {
    width: fit-content;
    font-size: 13px;
    margin-right: 10px;
}
.field .input {
    width: 100%;
    border-radius: 20px;
    padding: 5px;
    border: none;
    cursor: auto;
    box-shadow: inset 5px 5px 9px #5a5a5a,
        inset -5px -5px 9px #ffffff;
}
.field .input:focus-visible {
    outline: none;
}
.close {
    position: absolute;
    top: 10px;
    right: 10px;
    cursor: pointer;
    transition: 0.3s;
}
.close:hover {
    transform: scale(1.1);
}
.input {
    text-align: center;
}
</style>