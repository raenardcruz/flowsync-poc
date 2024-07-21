<template>
    <div class="process-content">
        <div class="filter-box">
            <div class="filter-title">Tags</div>
            <div class="filter-search">
                <input class="search-box" type="search" placeholder="Search" v-model="searchTags">
            </div>
            <div class="filter-checks">
                <span v-for="tag in filteredTags" :key="tag.name">
                    <CheckBox class="check" :label="tag.name" v-model="tag.value" />
                </span>
            </div>
        </div>
        <div class="gallery-box">
            <div class="page-title">Process Gallery</div>
            <div class="page-info">
                <span>Showing</span>
                <span style="color: #3990C5;">{{ filterdProcesses.length }}</span>
                <span>out of {{ processes.length }} Processes</span>
            </div>
            <div class="gallery-search">
                <input class="search-box" type="search" placeholder="Search" v-model="search">
            </div>
            <div class="cards-gallery">
                <div class="create" @click="newProcess()">
                    <span class="material-symbols-outlined">add</span>
                </div>
                <div class="card" v-for="process in filterdProcesses" :key="process.id" @click="selectTab(process.id)">
                    <div class="type">
                        <span
                            style="color: #14BF47; margin-right: 10px;"
                            class="material-symbols-outlined"
                            v-if="process.type == 'default'">flag_circle</span>
                        <span
                            style="color: #09a6d6; margin-right: 10px;"
                            class="material-symbols-outlined"
                            v-else-if="process.type == 'interval'">schedule</span>
                        <span
                            style="color: #b86a11; margin-right: 10px;"
                            class="material-symbols-outlined" 
                            v-else-if="process.type == 'webhook'">webhook</span>
                        <span
                            style="color: #A3245B; margin-right: 10px;"
                            class="material-symbols-outlined" 
                            v-else-if="process.type == 'events'">event</span>
                        <span
                            class="material-symbols-outlined"
                            style="color: #84ab86; margin-right: 10px;"
                            v-else-if="process.type=='default'">
                            radio_button_unchecked
                        </span>
                        <span class="type-text">{{ process.type }}</span>
                    </div>
                    <div class="card-title">
                        <span>{{ process.title }}</span>
                    </div>
                    <div class="card-description">{{ process.description }}</div>
                    <div class="tags">
                        <div class="tag" v-for="tab in process.tags">{{ tab }}</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import CheckBox from "../Common/CheckBox.vue";
import { copyObj } from "../scripts/helper";
import store from "../store"
import { getAllProcesses, selectTab, newProcess } from "./scripts/functions"
import { ref, computed, watch } from "vue"

const { processes, tags } = store();
const search = ref("")
const searchTags = ref("")
const filteredTags = computed(() => {
    if (searchTags.value.length > 0)
        return tags.value.filter(f => f.name.toLowerCase().includes(searchTags.value.toLowerCase()));
    else
        return tags.value;
});
const filterdProcesses = computed(() => {
    if (search.value.length > 0) {
        return processes.value.filter(f => 
                    (f.title.toLowerCase().includes(search.value.toLowerCase()) || 
                    f.type.toLowerCase().includes(search.value.toLowerCase()) ||
                    f.description.toLowerCase().includes(search.value.toLowerCase())) && tagFilter(f))
    }
    else {
        return processes.value.filter(f => {
           return tagFilter(f)   
        })
    }
})
const tagFilter = (obj) => {
    if (obj.tags.length > 0) {
        var tmp = new Set(tags.value.filter(f => f.value == true).map(m => m.name))
        return obj.tags.some(val => tmp.has(val))
    } else {
        return tags.value.find(f => f.name == "No Tags").value
    }
}
getAllProcesses();
</script>

<style scoped>
.process-content {
    position: relative;
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
    height: 100%;
    width: 100%;
    padding: 30px;
    background: #f0f0f0;

}
.filter-box {
    position: relative;
    display: flex;
    align-items: flex-center;
    justify-content: flex-start;
    flex-direction: column;
    border: 1px solid #fff;
    border-radius: 8px;
    width: 12%;
    padding: 10px;
    overflow: auto;
}
.filter-box .filter-title {
    color: #797272;font-weight: 500;
}
.filter-search {
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    width: 100%;
    height: 20px;
    border-radius: 20px;
    margin-top: 10px;
    box-shadow: 4px 4px 4px grey;
}
.filter-search .search-box {
    height: 100%;
    border-radius: 20px;
    padding: 0 10px 0 10px;
    border: none;
    width: inherit;
}
.filter-search .search-box:focus-visible {
    outline: none;
}
.filter-box .filter-checks {
    position: relative;
    display: flex;
    justify-content: flex-start;
    align-self: flex-start;
    flex-direction: column;
    margin-top: 30px;
}
.filter-box .filter-checks .check {
    margin-top: 10px;
}
.gallery-box {
    position: relative;
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    flex-direction: column;
    border: 1px solid #fff;
    border-radius: 8px;
    width: 85%;
    padding: 30px;
}
.page-title {
    font-size: 41px;
    color: #797272;
}
.page-info {
    font-size: 15px;
    color: #AD9A9A;
}
.page-info *  {
    margin-right: 5px;
}
.gallery-search {
    position: relative;
    display: flex;
    width: 100%;
    height: 30px;
    border-radius: 20px;
    margin-top: 30px;
    box-shadow: 4px 4px 4px grey;
}
.gallery-search .search-box {
    width: 100%;
    height: 100%;
    border-radius: 20px;
    padding: 0 10px 0 10px;
    border: none;
}
.gallery-search .search-box:focus-visible {
    outline: none;
}
.cards-gallery {
    position: relative;
    display: flex;
    flex-wrap: wrap;
    margin-top: 30px;
    gap: 20px;
}
.cards-gallery .create {
    position: relative;
    display: flex;
    height: 130px;
    width: 250px;
    justify-content: center;
    align-items: center;
    border-radius: 8px;
    background: #D9D9D9;
    color: #8E8B8B;
    cursor: pointer;
    overflow: hidden;
    transition: 0.3s ease-in-out;
}
.cards-gallery .create:hover {
    box-shadow: 4px 4px 4px grey;
}
.cards-gallery .card {
    position: relative;
    display: flex;
    height: 130px;
    width: 250px;
    padding: 10px;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    border-radius: 8px;
    background: #fff;
    cursor: pointer;
    overflow: hidden;
    box-shadow: 2px 2px 2px grey;
    transition: 0.3s ease-in-out;
}
.cards-gallery .card:hover {
    box-shadow: 6px 6px 6px rgb(116, 116, 116);
}
.cards-gallery .card .type {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 10px;
    text-transform: uppercase;
}
.cards-gallery .card .card-title {
    font-size: 13px;
    font-weight: 600;
}
.cards-gallery .card .card-description {
    font-size: 10px;
    color: grey;
}
.tags {
    position: relative;
    display: flex;
    flex-wrap: wrap;
    gap: 10px;
    margin-top: 10px;
}
.tags .tag {
    position: relative;
    display: flex;
    font-size: 10px;
    padding: 0 10px 0 10px;
    background: #3489CD;
    border-radius: 20px;
    color: #fff;
}
</style>