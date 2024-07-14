<template>
    <Teleport to=".page">
        <VariableProfileModal 
            :variable="model"
            :tabid="node.tabId"
            v-if="showModal"
            @close="showModal = false"></VariableProfileModal>
    </Teleport>
    <input
        type="text"
        spellcheck="false"
        v-model="model"
        :id="id"
        :class="class"
        :placeholder="placeholder"
        >
    <div class="profile-btn" @click="showModal = true" v-if="model?.length > 0">
        <span class="material-symbols-outlined" style="width: fit-content; color: #919899;">edit_document</span>
        (optional) Set Variable Profile
    </div>
</template>

<script setup>
import {ref, watch} from "vue"
import store from "../../store.js";
import VariableProfileModal from './VariableProfileModal.vue';

const props = defineProps(['node', 'class', 'placeholder', 'id']);
const model = defineModel();
const { tabs } = store();
const showModal = ref(false);
var tab = tabs.value.find(f => f.id == props.node.tabId);
watch(model, (newVal, oldVal) => {
    var searchResult = tab.variableProfiles.filter(f => f.name == oldVal);
    if (searchResult.length > 0)
        searchResult[0].name = newVal
})
</script>

<style scoped>
@import "../nodes/inputStyle.css";
.profile-btn {
    position: relative;
    display: flex;
    font-size: 10px;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
    width: 100%;
    gap: 10px;
    padding: 3px;
    border-radius: 20px;
    background: #f0f0f0;
    box-shadow:  3px 3px 5px #868686,
             -3px -3px 5px #ffffff;
}
.profile-btn:hover {
    box-shadow:  inset 3px 3px 5px #868686,
             inset -3px -3px 5px #ffffff;
}
</style>