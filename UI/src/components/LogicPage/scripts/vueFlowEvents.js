import { computed, watch } from "vue"
import { findTab } from "./tabFunctions";
import store from "../../store";
import { copyObj } from "../../scripts/helper";
import { v4 as uuidv4 } from 'uuid';

const {
    activeTab,
    showSideBar,
    draggedNode,
    mousePosition,
    clipBoard
} = store();

export default class VueFlowEvents {
    constructor(id, vueFlow) {
        this.tab = findTab(id)
        this.vueFlow = vueFlow
    }

    initializeEvents = () => {
        const {
            onConnect,
            addEdges,
            removeSelectedNodes,
            getSelectedNodes,
            nodesDraggable
        } = this.vueFlow;

        const runMode = computed(() => this.tab.runMode);
        nodesDraggable.value = !runMode.value;
        watch(runMode, (newValue) => nodesDraggable.value = !newValue);
        removeSelectedNodes(getSelectedNodes.value);
        onConnect((connection) => {
            if (!this.tab.runMode)
              addEdges(connection)
        })
    }

    resetTransform = () => {
        const {
            setViewport
        } = this.vueFlow;
        setViewport({ x: 0, y: 0, zoom: 1 })
    }

    onChangeNodes = (changes) => {
        if (activeTab.value != this.tab.id)
          return;
        changes.forEach(change => {
          if (change.type == "remove") {
            this.tab.nodes.splice(this.tab.nodes.findIndex(f => f.id == change.id), 1)
          } else if (change.type == "position") {
            if (change.position != null) this.tab.nodes.find(f => f.id == change.id).position = copyObj(change.position)
          }
        });
    }

    onChangeEdge = (changes) => {
        if (activeTab.value != this.tab.id)
          return;
        changes.forEach(change => {
          if (change.type == "add") {
            change.item.type = "custom";
            change.item.animated = true;
            change.item.tabId = this.tab.id;
            this.tab.edges.push(change.item);
          } else if (change.type == "remove") {
            this.tab.edges.splice(this.tab.edges.findIndex(f => f.id == change.id), 1);
          }
        });
    }

    onDrop = (event) => {
        const { screenToFlowCoordinate } = this.vueFlow;
        const position = screenToFlowCoordinate({
          x: event.clientX,
          y: event.clientY,
        })
        var newNode = copyObj(draggedNode.value);
        newNode.position = position;
        newNode.id = uuidv4();
        newNode.tabId = this.tab.id;
        this.tab.nodes.push(newNode);
        showSideBar.value = false;
    }

    handleMouseMove = (event) => {
        mousePosition.value.x = event.clientX;
        mousePosition.value.y = event.clientY;
    }

    handleKeyDown = (event) => {
        const {
            getSelectedNodes, 
            getSelectedEdges, 
            screenToFlowCoordinate,
            addSelectedNodes
        } = this.vueFlow;

        if (this.tab.runMode) {
          event.preventDefault();
          return;
        }
        var tagName = event.srcElement.tagName;
        var isInput = tagName == 'TEXTAREA' || tagName == 'INPUT';
      
        // Copy action Ctrl + C
        if (event.ctrlKey && event.key === 'c') {
          if (!isInput) {
            event.preventDefault();
            clipBoard.value = {
              nodes: copyObj(getSelectedNodes.value),
              edges: copyObj(getSelectedEdges.value)
            };
          }
        }
      
        // Paste action Ctrl + V
        else if (event.ctrlKey && event.key === 'v') {
          if (!isInput) {
            event.preventDefault();
            var newNodes = [];
            var newEdges = "";
            var counter = 0;
            var referenceX = 0;
            var referenceY = 0;
            var offsetX = 0;
            var offsetY = 0;
            newEdges = JSON.stringify(clipBoard.value.edges);
            clipBoard.value.nodes.forEach(nodeClip => {
              const position = screenToFlowCoordinate({
                x: mousePosition.value.x,
                y: mousePosition.value.y,
              })
              if (counter == 0) {
                referenceX = nodeClip.position.x;
                referenceY = nodeClip.position.y;
              } else {
                offsetX = nodeClip.position.x - referenceX;
                offsetY = nodeClip.position.y - referenceY;
                position.x = position.x + offsetX;
                position.y = position.y + offsetY;
              }
              var newNode = copyObj(nodeClip);
              newNode.id = uuidv4();
              newNode.position = position;
              newNode.tabId = this.tab.id;
              this.tab.nodes.push(newNode);
              newEdges = newEdges.replaceAll(nodeClip.id.toString(), newNode.id.toString());
              newNodes.push(newNode);
              counter++;
            });
            JSON.parse(newEdges).forEach(edgeClip => {
              if (newNodes.filter(f => f.id == edgeClip.target).length > 0) {
                edgeClip.tabId = this.tab.id;
                this.tab.edges.push(edgeClip);
              }
            });
          }
        }
      
        // Select all Nodes Ctrl + A
        else if (event.ctrlKey && event.key === 'a') {
          if (!isInput) {
            event.preventDefault();
            event.preventDefault();
            addSelectedNodes(this.tab.nodes.filter(f => f.id != "1"))
          }
        }
      
        // Tab event
        else if (event.key == 'Tab') {
          if (tagName == 'TEXTAREA') {
            event.preventDefault();
            const textarea = event.target;
            const start = textarea.selectionStart;
            const end = textarea.selectionEnd;
            const tabSpaces = '    ';
            textarea.value = textarea.value.substring(0, start) + tabSpaces + textarea.value.substring(end);
            textarea.selectionStart = textarea.selectionEnd = start + tabSpaces.length;
          }
        }
      
        // Backspace event
        else if (event.key == 'Backspace') {
          if (!isInput) {
            event.preventDefault();
            getSelectedNodes.value.forEach(node => {
              if (node.id == "1")
                return;
              this.tab.nodes.splice(this.tab.nodes.findIndex(f => f.id == node.id), 1);
            })
          }
        }
      
        // Shift event
        else if (event.key == 'Shift') {
          event.preventDefault();
        }
      }
}