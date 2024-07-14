import SetVariableNode from "../nodes/SetVariableNode.vue";
import StartNode from "../nodes/StartNode.vue";
import ConditionNode from "../nodes/ConditionNode.vue";
import LoopNode from "../nodes/LoopNode.vue";
import ForEachNode from "../nodes/ForEachNode.vue";
import WhileNode from "../nodes/WhileNode.vue";
import ApiNode from "../nodes/ApiNode.vue";
import LogNode from "../nodes/LogNode.vue";
import TextNode from "../nodes/TextNode.vue";
import GetGuidNode from "../nodes/GetGuidNode.vue";
import MathNode from "../nodes/MathNode.vue";
import ListNode from "../nodes/ListNode.vue";
import CountNode from "../nodes/CountNode.vue";
import MapNode from "../nodes/MapNode.vue";
import ReplaceNode from "../nodes/ReplaceNode.vue";
import RegexFindNode from "../nodes/RegexFindNode.vue";
import ImageNode from "../nodes/ImageNode.vue";

const nodeTypes = {
    start: StartNode,
    setVariable: SetVariableNode,
    condition: ConditionNode,
    loop: LoopNode,
    foreach: ForEachNode,
    while: WhileNode,
    api: ApiNode,
    log: LogNode,
    getGuid: GetGuidNode,
    text: TextNode,
    math: MathNode,
    list: ListNode,
    count: CountNode,
    map: MapNode,
    replace: ReplaceNode,
    regexfind: RegexFindNode,
    image: ImageNode
};

export default nodeTypes;