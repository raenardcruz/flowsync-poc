<template>
    <div class="background">
        <Notif ref="notify" />
        <div class="modal">
            <div class="close-btn" @click="close">
                <span class="material-symbols-outlined">close</span>
            </div>
            <div class="title">
                <span class="material-symbols-outlined">edit_document</span>
                Variable Profile
            </div>
            <div class="instructions">Enter the sample JSON format for variable {<span class="reserved-word">{{ variable }}</span>}</div>
            <textarea
                ref="textarea"
                class="json-profile"
                list="suggestions"
                placeholder="Enter your JSON Profile here"
                v-model="profile"
                @keydown.tab="handleTab"></textarea>
                <datalist id="suggestions">
                <option value="apple"></option>
                <option value="banana"></option>
                <option value="orange"></option>
                <option value="mango"></option>
                </datalist>
                <div class="save-btn" @click="addToVariableProfiles">
                    <span class="material-symbols-outlined">save</span>
                    Save Profile
                </div>
        </div>
    </div>
</template>

<script setup>
import {ref, nextTick} from "vue"
import store from "../../store.js";
import Notif from "../../Common/Notif.vue";

const notify = ref(null);
const profile = ref("")
const textarea = ref(null)
const { tabs } = store();
const props = defineProps(['variable', 'tabid'])
const emit = defineEmits(['update:modelValue', 'close']);
const model = defineModel();
var tab = tabs.value.find(f => f.id == props.tabid);

var varText = tab.variableProfiles.find(f => f.name == props.variable);
profile.value = JSON.stringify(varText?.value, null, 2);

const close = function () {
    emit('close');
}
const handleTab = function (event) {
    event.preventDefault()
    var lastPosition = textarea.value.selectionStart;
    profile.value = profile.value.slice(0, lastPosition) + "  " + profile.value.slice(lastPosition)
    nextTick(() => {
        textarea.value.selectionStart = lastPosition + 2
        textarea.value.selectionEnd = lastPosition + 2
    })
}
const validateJson = function () {
    try {
        JSON.parse(profile.value);
        return true;
    } catch (error) {
        notify.value.show("Not a valid JSON", "error");
        return false;
    }
}
const addToVariableProfiles = function () {
    if (validateJson()) {
        var varProfile = tab.variableProfiles.filter(f => f.name == props.variable);
        if (varProfile.length == 0) {
            tab.variableProfiles.push({
                name: props.variable,
                value: JSON.parse(profile.value)
            })
        } else {
            varProfile[0].value = JSON.parse(profile.value);
        }
        emit('close');
    }
}
</script>

<style scoped>
.background {
    position: absolute;
    display: flex;
    height: 100%;
    width: 100%;
    justify-content: center;
    align-items: center;
    background: rgba(0, 0, 0, 0.525);
    z-index: 100;
}
.background .modal {
    position: relative;
    display: flex;
    flex-direction: column;
    height: 50vh;
    width: 60vw;
    min-height: 100px;
    min-width: 300px;
    max-width: 500px;
    border-radius: 10px;
    background: #fff;
    padding: 20px;
    overflow: auto;
    animation: modalentry 0.5s;
    user-select: text;
}
.close-btn {
    position: absolute;
    right: 10px;
    top: 10px;
    cursor: pointer;
    transition: 0.2s linear;
}
.close-btn:hover {
    color: red;
}
.instructions {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 13px;
    color: grey;
}
.json-profile {
    height: 100%;
    border-radius: 20px;
    padding: 10px;
    margin-top: 20px;
}
.json-profile:focus-visible {
    outline: none;
    border: 0.05px solid #1092B3;
}
.save-btn {
    position: relative;
    display: flex;
    gap: 10px;
    width: 100%;
    height: 30px;
    margin: 5px 0 5px 0;
    justify-content: center;
    align-items: center;
    font-size: 15px;
    border-radius: 20px;
    background: rgb(3, 161, 213);
    color: #fff;
    cursor: pointer;
    transition: 0.2s linear;
}
.save-btn:hover {
    box-shadow: 4px 4px 5px grey, 0 0 5px rgb(3, 161, 213), 0 0 8px rgb(3, 161, 213);
}

@keyframes modalentry {
    from {
        transform: translateY(20px);
    } 
    to {
        transform: translateY(0px);
    }
}
</style>