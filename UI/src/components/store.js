import { ref, shallowRef } from "vue";

const draggedNode = ref(null);
const clipBoard = ref([]);
const mousePosition = ref({
  x: 0,
  y: 0
})
const showSideBar = ref(false);
const activeTab = ref('main');
const tabs = ref([]);
const processes = ref([]);

export default function store() {
  return {
    draggedNode,
    mousePosition,
    clipBoard,
    showSideBar,
    activeTab,
    tabs,
    processes
  }
}