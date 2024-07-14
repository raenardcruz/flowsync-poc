<template>
    <div class="process-content">
        <div class="page-title">Process Gallery</div>
        <div class="page-info">
            <span>Showing</span>
            <span style="color: #3990C5;">2</span>
            <span>out of 2 Processes</span>
        </div>
        <div class="search">
            <input class="search-box" type="text" placeholder="Search">
            <div class="search-btn">Search</div>
        </div>
        <div class="cards-gallery">
                <div class="create" @click="newProcess()">
                    <span class="material-symbols-outlined">add</span>
                </div>
                <div class="card" v-for="process in processes" :key="process.id" @click="selectTab(process.id)">
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
                </div>
            </div>
    </div>
</template>

<script setup>
import store from "../store"
import { getAllProcesses, selectTab, newProcess } from "./scripts/functions"

const { processes } = store();
getAllProcesses();
</script>

<style scoped>
.process-content {
    position: relative;
    display: flex;
    flex-direction: column;
    height: 100%;
    width: 100%;
    background: #f0f0f0;

}
.page-title {
    position: relative;
    display: flex;
    font-size: 32px;
    margin: 14px 0 0 174px;
    color: #797272;
}
.page-info {
    position: relative;
    display: flex;
    gap: 5px;
    font-size: 12px;
    color: #AD9A9A;
    margin: 10px 0 0 174px;
}
.search {
    position: relative;
    display: flex;
    margin: 14px 0 0 174px;
}
.search .search-box {
    border-radius: 20px;
    border: none;
    height: 30px;
    width: 460px;
    box-shadow: 0px 4px 4px grey;
    padding: 5px 20px 5px 20px;
}
.search .search-box:focus-visible {
    outline: none;
}
.search .search-btn {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 20px;
    border: none;
    height: 30px;
    width: 123px;
    font-size: 13px;
    background: #3990C5;
    color: #fff;
    margin-left: 10px;
    cursor: pointer;
    transition: 0.3s linear;
}
.search .search-btn:hover {
    filter: brightness(1.2);
}
.cards-gallery {
    position: relative;
    display: flex;
    margin: 50px 174px 0 174px;
    flex-wrap: wrap;
    gap: 25px;
}
.cards-gallery .create {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 154px;
    width: 269px;
    background: #D9D9D9;
    border-radius: 10px;
    cursor: pointer;
    transition: 0.3s linear;
}
.cards-gallery .create:hover {
    box-shadow: 0px 4px 4px grey;
}
.cards-gallery .create .material-symbols-outlined {
    font-size: 64px;
    color: #8E8B8B;
}
.cards-gallery .card {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 154px;
    width: 269px;
    border-radius: 10px;
    cursor: pointer;
    background: #fff;
    box-shadow: 0px 4px 4px grey;
    transition: 0.3s linear;
    padding: 20px;
}
.cards-gallery .card:hover {
    transform: scale(1.02);
    box-shadow:  6px 6px 20px #868686,
             -6px -6px 20px #ffffff;
}
.cards-gallery .card .card-title {
    font-size: 20px;
}
.cards-gallery .card .card-description {
    font-size: 13px;
    color: #868686;
}
.type {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
}
.type .type-text {
    font-size: 13px;
    color: gray;
    text-transform: uppercase;
}
</style>