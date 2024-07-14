<template>
    <input
        type="text"
        spellcheck="false"
        v-model="model"
        ref="messageInput"
        :id="id"
        :class="class"
        :placeholder="placeholder"
        @input="handleChange($event.target.value)"
        @keydown.ctrl.space="showHelpvariables"
        @mouseover="hover=true" 
        @mouseleave="hover=false"
        >
    <div id="hover-msg" class="hover-msg" v-html="formatText" v-if="hover"></div>
    <div class="search" v-if="isSearch">
        <div class="title">
            <span class="material-symbols-outlined" style="width: fit-content; margin-right: 10px; color: red;">help</span>
            Dynamic Variable Lookup
        </div>
        <input
        type="text"
        ref="searchInput"
        v-model="searchText"
        placeholder="Search"
        @keydown="handleKeyPress">
        <div
        class="btn" 
        v-for="(variable, index) in filteredVariables" 
        :key="variable"
        :class="{'selected': selectedVariable == index}"
        @click=handleVariableSelect(variable.value)>
            <span class="material-symbols-outlined type" style="color: #0D78B2;" v-if="variable.type == 'string'">abc</span>
            <span class="material-symbols-outlined type" style="color: #C83E3E;" v-else-if="variable.type == 'number'">123</span>
            <span class="material-symbols-outlined type" style="color: #EC8605;" v-else-if="variable.type == 'object'">data_object</span>
            <span class="material-symbols-outlined type" v-else>question_mark</span>
            {{ variable.name }}
        </div>
    </div>
</template>

<script setup>
import { ref, computed, nextTick  } from 'vue';
import store from '../../store';

const props = defineProps(['node', 'class', 'placeholder', 'id']);
const model = defineModel();
const emit = defineEmits(['update:modelValue']);
const hover = ref(false)
const isSearch = ref(false);
const messageInput = ref(null);
const searchInput = ref(null);
const selectedVariable = ref(0);
const searchText = ref("");
const lastPositionn = ref(0);
const { tabs } = store()

var tab = tabs.value.find(f => f.id == props.node.tabId)

const keyType = function (key, obj) {
    if (obj != null) {
        if (key.length > 0) {
            if (obj[key] != null)
                return typeof obj[key];
            else
                return ""
        }
        else
            return typeof obj;
    }
    else
        return ""
}
const listKeys = function (obj, keyArray) {
    var parent = "";
    var search = keyArray[keyArray.length - 1];
    for (var index = 0; index < keyArray.length - 1; index++) {
        parent += parent.length > 0 ? "." : "";
        parent += keyArray[index];
    }
    var tmpObj = obj[0].value;
    for (var index = 1; index < keyArray.length - 1; index++) {
        tmpObj = tmpObj[keyArray[index]];
    }
    if (typeof tmpObj == 'object') {
        var keys = Object.keys(tmpObj);
        return keys.filter(f => f.includes(search)).map(m => {
            return {
                name: m,
                value: `${parent}.${m}`,
                type: keyType(m, tmpObj)
            }
        })
    } else
        return []
}
const variables = computed(() => {
    var tmp = [];
    tmp.push("currentdata");
    var filtObj = tab.nodes.filter(f => f.type == "setVariable");
    filtObj.forEach(obj => tmp.push(obj.data.name));
    filtObj = tab.nodes.filter(f => f.data?.variable != null);
    filtObj.forEach(obj => tmp.push(obj.data?.variable));
    return tmp;
});
const filteredVariables = computed(() => {
    if (searchText.value == null) {
        var tmp = [...new Set(variables.value.filter(f => f.length > 0))];
        if(tmp.length > 0 )
            selectedVariable.value = 0
        return tmp.map(m => {
            return {
                name: m,
                value: m,
                type: keyType("", tab.variableProfiles.find(f => f.name == m)?.value)
            }
        });
    }
    else {
        var splitSearch = searchText.value.split(".");
        if (splitSearch.length ==1) {
            var tmp = [...new Set(variables.value.filter(f => f.length > 0).filter(f => f.includes(searchText.value)))]
            if(tmp.length > 0 )
                selectedVariable.value = 0
            return tmp.map(m => {
                return {
                    name: m,
                    value: m,
                    type: keyType("", tab.variableProfiles.find(f => f.name == m)?.value)
                }
            });
        } else if (splitSearch.length > 1) {
            var keys = listKeys(tab.variableProfiles, splitSearch)
            return keys;
        } else {
            return [];
        }
    }
});
const formatText = computed(() => {
    let text = model.value;
    const spanClass = 'reserved-word';
    const isAlreadyWrapped = (str, word) => {
        const wrapRegex = new RegExp(`<span class="${spanClass}">\\{${word}[^}]*\\}</span>`);
        return wrapRegex.test(str);
    };
    variables.value.forEach(word => {
        if (word.length > 0) {
            const regex = new RegExp(`\\{${word}[^}]*\\}`, 'g');
            text = text.replace(regex, match => {
                if (!isAlreadyWrapped(text, word)) {
                    return `<span class="${spanClass}">${match}</span>`;
                }
                return match;
            });
        }
    });
    return text;
});


const showHelpvariables = function () {
    isSearch.value = true;
    nextTick(() => {
        lastPositionn.value = messageInput.value.selectionStart;
        searchInput.value.focus();
    })
}

const handleKeyPress = function (event) {
  const keyPressed = event.key;
  if (isSearch.value) {
    if (keyPressed == 'Enter') {
        event.preventDefault();
        if (filteredVariables.value.length > 0) {
            handleVariableSelect(filteredVariables.value[selectedVariable.value].value)
            nextTick(() => messageInput.value.focus())
        }
    } else if (keyPressed == 'Escape') {
        isSearch.value = false;
        searchText.value = "";
        nextTick(() => messageInput.value.focus())
    } else if (keyPressed == 'ArrowDown') {
        event.preventDefault();
        if (selectedVariable.value >= filteredVariables.value.length - 1)
            selectedVariable.value = 0;
        else
            selectedVariable.value++;
    } else if (keyPressed == 'ArrowUp') {
        event.preventDefault();
        if (selectedVariable.value <=  0)
            selectedVariable.value = filteredVariables.value.length - 1;
        else
            selectedVariable.value--;
    } else if (keyPressed == 'Tab') {
        event.preventDefault();
        var selectedValue = filteredVariables.value[selectedVariable.value];
        if (selectedValue.type == 'object')
            searchText.value = selectedValue.value + "."
        else
            searchText.value = selectedValue.value
    }
  }
}
const handleChange = function (newValue) {
  model.value = newValue;
  emit('update:modelValue', newValue);
}
const handleVariableSelect = function (text) {
    model.value = model.value.slice(0, messageInput.value.selectionStart) + `{${text}}` + model.value.slice(messageInput.value.selectionStart)
    isSearch.value = false;
    searchText.value = "";
    nextTick(() => {
        messageInput.value.selectionStart = lastPositionn.value + `{${text}}`.length
        messageInput.value.selectionEnd = lastPositionn.value  + `{${text}}`.length
    })
}
</script>

<style scoped>
@import "../nodes/inputStyle.css";
.hover-msg {
    position: absolute;
    display: flex;
    flex-wrap: wrap;
    text-wrap: wrap;
    line-break: auto;
    padding: 10px;
    font-size: 10px;
    background: rgba(0, 0, 0, 0.753);
    color: #fff;
    top: 120%;
    z-index: 100;
    border-radius: 8px;
    width: fit-content !important;
}
.search {
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 10px;
    padding: 10px;
    font-size: 10px;
    color: #222;
    left: 110%;
    flex-direction: column;
    border-radius: 8px;
    z-index: 100;
    background: #f0f0f0;
    width: fit-content !important;
    box-shadow:  6px 6px 12px #868686,
             -6px -6px 12px #ffffff;
}
.btn {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 20px;
    border-radius: 10px;
    font-size: 10px;
    transition: 0.3s;
    background: #f0f0f0;
    box-shadow:  6px 6px 12px #868686,
             -6px -6px 12px #ffffff;
}
.btn:hover {
    background: #f0f0f0;
    box-shadow:  inset 6px 6px 12px #868686,
             inset -6px -6px 12px #ffffff;
}
.btn.selected {
    background: #f0f0f0;
    box-shadow:  inset 6px 6px 12px #868686,
             inset -6px -6px 12px #ffffff;
}
.title {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: 600;
    text-wrap: nowrap;
}
.type {
    width: fit-content !important;
    font-size: 12px;
    margin-right: 5px;
}
</style>