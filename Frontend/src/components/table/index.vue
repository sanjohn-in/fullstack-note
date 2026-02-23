<template>
	<el-table
		ref="tableRef"
		:data="filterTableData"
		:border="setBorder"
		v-bind="$attrs"
		row-key="id"
		stripe
		style="width: 100%"
		v-loading="config.loading"
		@selection-change="onSelectionChange"
		sortable
	>
		<el-table-column
			type="selection"
			:reserve-selection="true"
			width="30"
			v-if="config.isSelection"
		/>
		<el-table-column type="index" :label="'No'" width="60" />
		<el-table-column
			v-for="(item, index) in setHeader"
			:key="index"
			show-overflow-tooltip
			:prop="item.key"
			:width="item.colWidth"
			:label="item.title"
		>
			<template v-slot="scope">
				<template v-if="item.type === 'image'">
					<el-image
						:style="{
							width: `${item.width}px`,
							height: `${item.height}px`,
							borderRadius: `6px`,
						}"
						:src="scope.row[item.key]"
						:zoom-rate="1.2"
						:preview-src-list="[scope.row[item.key]]"
						preview-teleported
						fit="cover"
					/>
				</template>
				<template v-else-if="item.type === 'switch'">
					<el-switch
						@change="item?.fun && item?.fun(scope.row, $event)"
						v-model="scope.row[item.key]"
						inline-prompt
						:active-text="item.activeText || 'yes'"
						:inactive-text="item.activeText || 'no'"
						:active-value="!item.activeValue || 0"
						:inactive-value="item.inactiveValue || 1"
					></el-switch>
				</template>
				<template v-else-if="item.type === 'date'">
					<div v-if="scope.row[item.key]">
						{{ dayjs(scope.row[item.key]).format("YYYY-MM-DD") }}
					</div>
				</template>
				<template v-else-if="item.type === 'tag'">
					<el-tag type="success" v-if="scope.row[item.key]">{{ "yes" }}</el-tag>
					<el-tag type="info" v-else>{{ "no" }}</el-tag>
				</template>
				<template v-else-if="item.type === 'color'">
					<span :style="{ color: scope.row.color }">
						{{ scope.row.color }}
					</span>
				</template>
				<template v-else-if="item.type === 'link'">
					<el-link type="primary" :underline="false">
						{{ scope.row.link }}</el-link
					>
				</template>
				<template v-else-if="item.type === 'any'">
					<div v-if="scope.row[item.key] === 'image'">
						<el-image
							:style="{
								width: `${item.width ? item.width : 120}px`,
								height: `${item.height ? item.height : 40}px`,
							}"
							:src="scope.row.value"
							:zoom-rate="1.2"
							:preview-src-list="[scope.row.value]"
							preview-teleported
							fit="cover"
						/>
					</div>
					<div v-else-if="scope.row[item.key] === 'editor'">
						<span class="truncate line-clamp-6" v-html="scope.row.value" />
					</div>
					<div v-else>{{ scope.row.value }}</div>
				</template>
				<template v-else-if="item.type === 'array'">
					<div class="grid grid-cols-2 gap-2">
						<el-text
							type="primary"
							v-for="(date, index) in scope.row[item.key]"
							:key="index"
							>{{ date }}</el-text
						>
					</div>
				</template>
				<template v-else>
					<div
						@click="item?.fun && item?.fun(scope.row)"
						:class="{ haveFun: item?.fun }"
						:style="{
							color: item.keyArray
								? ['blue', 'green', 'red'][scope.row[item.key]]
								: '',
						}"
					>
						{{
							item.keyArray && item.keyArray[scope.row[item.key]]
								? item.keyArray[scope.row[item.key]]
								: scope.row[item.key]
						}}
					</div>
				</template>
			</template>
		</el-table-column>
		<el-table-column
			:label="'operate'"
			min-width="180"
			v-if="config.isOperate?.length"
			fixed="right"
		>
			<template #header>
				<el-input v-model="state.search" size="default" :placeholder="'name'" />
			</template>
			<template v-slot="scope">
				<div style="display: flex">
					<div
						v-for="(item, index) in config.isOperate"
						class="mr10"
						:key="index"
					>
						<el-popconfirm
							v-if="item.type == 'tip'"
							:title="item.tipTitle"
							@confirm="onSystem(scope.row, item.key)"
						>
							<template #reference>
								<el-button
									size="small"
									v-if="item.type == 'tip'"
									text
									type="danger"
									>{{ item.label }}</el-button
								>
							</template>
						</el-popconfirm>
						<el-button
							size="small"
							v-else
							text
							type="warning"
							@click="onSystem(scope.row, item.key)"
						>
							{{ item.label }}
						</el-button>
					</div>
				</div>
			</template>
		</el-table-column>
		<template #empty>
			<el-empty :description="'noData'" />
		</template>
	</el-table>
	<div class="flex justify-between mt-4">
		<el-pagination
			@size-change="onHandleSizeChange"
			@current-change="onHandleCurrentChange"
			:page-sizes="[10, 25, 50, 75, 100]"
			:small="true"
			v-model:current-page="state.page.pageNum"
			background
			v-model:page-size="state.page.pageSize"
			layout="total, sizes, prev, pager, next, jumper"
			:total="total"
		>
		</el-pagination>
		<div class="table-footer-tool">
			<!-- Print -->
			<el-tooltip content="Print" placement="top">
				<el-icon size="20" class="tool-icon" @click="onPrintTable">
					<Printer />
				</el-icon>
			</el-tooltip>

			<!-- Export -->
			<el-tooltip content="Export" placement="top">
				<el-icon size="22" class="tool-icon" @click="onImportTable">
					<Download />
				</el-icon>
			</el-tooltip>

			<!-- Refresh -->
			<el-tooltip content="Refresh" placement="top">
				<el-icon size="22" class="tool-icon" @click="onRefreshTable">
					<Refresh />
				</el-icon>
			</el-tooltip>

			<!-- Settings -->
			<el-popover
				placement="top-end"
				trigger="click"
				transition="el-zoom-in-top"
				popper-class="table-tool-popper"
				:width="300"
				:persistent="false"
				@show="onSetTable"
			>
				<template #reference>
					<el-tooltip content="Settings" placement="top">
						<el-icon size="22" class="tool-icon">
							<Setting />
						</el-icon>
					</el-tooltip>
				</template>

				<template #default>
					<div class="tool-box overflow-auto">
						<!-- Help -->
						<el-tooltip content="Drag to sort" placement="top-start">
							<el-icon size="17" class="ml11 text-muted">
								<QuestionFilled />
							</el-icon>
						</el-tooltip>

						<el-checkbox
							v-model="state.checkListAll"
							:indeterminate="state.checkListIndeterminate"
							class="ml10 mr1"
							label="Column Display"
							@change="onCheckAllChange"
						/>

						<el-checkbox
							v-model="getConfig.isSelection"
							class="ml10 mr1"
							label="Multiple Choice"
						/>

						<el-checkbox
							v-model="getConfig.isSerialNo"
							class="ml10 mr1 pr-2"
							label="Serial Number"
						/>
					</div>

					<el-scrollbar>
						<div ref="toolSetRef" class="tool-sortable">
							<div
								class="tool-sortable-item"
								v-for="v in header"
								:key="v.key"
								:data-key="v.key"
							>
								<!-- Drag handle -->
								<el-icon class="handle cursor-pointer">
									<Rank />
								</el-icon>

								<el-checkbox
									v-model="v.isCheck"
									size="default"
									class="ml12 mr8"
									:label="v.title"
									@change="onCheckChange"
								/>
							</div>
						</div>
					</el-scrollbar>
				</template>
			</el-popover>
		</div>
	</div>
</template>

<script setup lang="ts" name="netxTable">
import dayjs from "dayjs";
import table2excel from "js-table2excel";
import printJs from "print-js";
import Sortable from "sortablejs";
import { computed, nextTick, onMounted, reactive, ref, watch } from "vue";
// import "/@/theme/tableTool.scss";
import { ElMessage } from "element-plus";

// Define the value passed by the parent component
const props = defineProps({
	// List content
	data: {
		type: Array<EmptyObjectType>,
		default: () => [],
	},
	// Header content
	header: {
		type: Array<EmptyObjectType>,
		default: () => [],
	},
	// Configuration items
	config: {
		type: Object,
		default: () => {
			return {
				isBorder: false, // Whether to show borders
				isSelection: false, // Whether to show selection column
				isSerialNo: false, // Whether to show serial number column
				keySearch: "name", // Search key
			};
		},
	},
	total: {
		type: Number,
		default: () => {},
	},
	// Print title
	printName: {
		type: String,
		default: () => "",
	},
	draggable: {
		type: Boolean,
		default: false,
	},
});

const tableRef = ref();
// Define child components to pass values to parent components
const emit = defineEmits([
	"onSystem",
	"pageChange",
	"sortHeader",
	"rowDragEnd",
]);

// Define variable content
const toolSetRef = ref();
// const { themeConfig } = storeToRefs(storesThemeConfig);
const state = reactive({
	page: {
		page: 1,
		pageSize: 10,
		pageNum: 1,
	},
	search: "",
	selectList: [] as EmptyObjectType[],
	checkListAll: true,
	checkListIndeterminate: false,
});
// Set border display/hide
const setBorder = computed(() => {
	return !!props.config?.isBorder;
});
// Get parent component configuration items (required)
const getConfig = computed((): EmptyObjectType => {
	return props?.config || {};
});
// Set tool header data
const setHeader = computed(() => {
	return props.header.filter((v) => v.isCheck);
});
// When tool column display select all changes
const onCheckAllChange = <T,>(val: T) => {
	if (val) props.header.forEach((v) => (v.isCheck = true));
	else props.header.forEach((v) => (v.isCheck = false));
	state.checkListIndeterminate = false;
};
// The tool column displays when the current item changes
const onCheckChange = () => {
	const headers = props.header.filter((v) => v.isCheck).length;
	state.checkListAll = headers === props.header.length;
	state.checkListIndeterminate = headers > 0 && headers < props.header.length;
};
// Used for export when table multi-select changes
const onSelectionChange = (val: EmptyObjectType[]) => {
	state.selectList = val;
};
// Delete current item
const onSystem = (row: EmptyObjectType, key: string) => {
	emit("onSystem", row, key);
};
// Pagination changes
const onHandleSizeChange = (val: number) => {
	state.page.pageSize = val;
	emit("pageChange", state.page);
};
// Pagination changes
const onHandleCurrentChange = (val: number) => {
	state.page.page = val;
	emit("pageChange", state.page);
};
// When searching, paging is restored to default
const pageReset = () => {
	state.page.page = 1;
	state.page.pageSize = 10;
	emit("pageChange", state.page);
};
// Print
const onPrintTable = () => {
	// https://printjs.crabbly.com/#documentation
	// Custom printing
	let tableTh = "";
	let tableTrTd = "";
	let tableTd: any = {};
	// Header
	props.header.forEach((v) => {
		tableTh += `<th class="table-th">${v.title}</th>`;
	});
	// Table content
	props.data.forEach((val, key) => {
		if (!tableTd[key]) tableTd[key] = [];
		props.header.forEach((v) => {
			if (v.type === "text") {
				tableTd[key].push(
					`<td class="table-th table-center">${val[v.key]}</td>`
				);
			} else if (v.type === "image") {
				tableTd[key].push(
					`<td class="table-th table-center"><img src="${
						val[v.key]
					}" onerror="this.onerror=null; this.src='/empty.png'" style="width:${
						v.width
					}px;height:${v.height}px;" /></td>`
				);
			} else if (v.type === "date") {
				tableTd[key].push(
					`<td class="table-th table-center" style="width: 100px">${dayjs(
						val[v.key]
					).format("YYYY-MM-DD")}</td>`
				);
			} else if (v.type === "any") {
				if (val[v.key] === "image") {
					tableTd[key].push(`<td class="table-th table-center"> <img
              style="width:${v.width ? v.width : 80}px;height:${
								v.height ? v.height : 40
							}px;"
              src="${
								val.value
							}" onerror="this.onerror=null; this.src='/empty.png'"
              zoom-rate="1.2"
          /></td>`);
				} else if (val[v.key] === "editor") {
					tableTd[key].push(
						`<td class="table-th table-center"><span v-html="${val.value}"></span></td>`
					);
				} else {
					tableTd[key].push(
						`<td class="table-th table-center">${val.value}</td>`
					);
				}
			} else {
				tableTd[key].push(
					`<td class="table-th table-center">${val[v.key]}</td>`
				);
			}
		});
		tableTrTd += `<tr>${tableTd[key].join("")}</tr>`;
	});
	// Print
	printJs({
		printable: `<div style=display:flex;flex-direction:column;text-align:center><h3>${props.printName}</h3></div><table border=1 cellspacing=0><tr>${tableTh}${tableTrTd}</table>`,
		type: "raw-html",
		css: [
			"//at.alicdn.com/t/c/font_2298093_rnp72ifj3ba.css",
			"//unpkg.com/element-plus/dist/index.css",
		],
		style: `@media print{.mb15{margin-bottom:15px;}.el-button--small i.iconfont{font-size: 12px !important;margin-right: 5px;}}; .table-th{word-break: break-all;white-space: pre-wrap;}.table-center{text-align: center;}`,
	});
};
// Export
const onImportTable = async () => {
	if (state.selectList.length <= 0)
		return ElMessage.warning("pleaseSelectDataToExport");
	let data: EmptyObjectType[] = [];
	props.header?.forEach((e) => {
		let info = e;
		info.title = info.title;
		info.type = info.type == "image" ? "image" : "text";
		data.push(e);
	});
	const stateValue = state.selectList.map((item) => {
		return { ...item, created_at: dayjs(item.created_at).format("YYYY-MM-DD") };
	});
	table2excel(data, stateValue, `${props.printName || "Export Data"}.xlsx`);
};
// refresh
const onRefreshTable = () => {
	emit("pageChange", state.page);
};
// set up
const onSetTable = () => {
	nextTick(() => {
		const sortable = Sortable.create(toolSetRef.value, {
			handle: ".handle",
			dataIdAttr: "data-key",
			animation: 150,
			onEnd: () => {
				const headerList: EmptyObjectType[] = [];
				sortable.toArray().forEach((val: string) => {
					props.header.forEach((v) => {
						if (v.key === val) headerList.push({ ...v });
					});
				});
				emit("sortHeader", headerList);
			},
		});
	});
};
const filterTableData = computed(() =>
	props.data.filter(
		(data) =>
			!state.search ||
			data[props.config.keySearch]
				.toLowerCase()
				.includes(state.search.toLowerCase())
	)
);
function arrayMove(arr: EmptyArrayType, fromIndex: number, toIndex: number) {
	const newArr = arr.slice();
	const element = newArr.splice(fromIndex, 1)[0];
	newArr.splice(toIndex, 0, element);
	return newArr;
}

// Setup draggable for table rows
function initTableDraggable() {
	nextTick(() => {
		if (!props.draggable) return;
		// Find the tbody element inside Element Plus Table
		// el-table uses a shadow DOM, so we need to get the actual DOM table
		let tbody;
		// Element Plus 2.x: tableRef.value.$el.querySelector('tbody')
		// If using Vue 3 ref, may be .value
		if (tableRef.value?.$el) {
			tbody = tableRef.value.$el.querySelector("tbody");
		} else if (tableRef.value) {
			tbody = tableRef.value.querySelector("tbody");
		}
		if (!tbody) return;

		// Destroy previous sortable if any
		if (tbody._sortable) {
			tbody._sortable.destroy();
			delete tbody._sortable;
		}

		tbody._sortable = Sortable.create(tbody, {
			animation: 180,
			handle: "tr", // allow drag via entire row
			ghostClass: "sortable-ghost",
			onEnd: (evt: EmptyObjectType) => {
				// Reorder the data array
				const oldIndex = evt.oldIndex;
				const newIndex = evt.newIndex;
				if (oldIndex !== newIndex) {
					// You must update the parent data for the table!
					const newOrder = arrayMove(props.data, oldIndex, newIndex);
					// Emit event with new order
					console.log("Row moved from", oldIndex, "to", newOrder);
					emit("rowDragEnd", newOrder);
				}
			},
		});
	});
}

// Watch for draggable prop
watch(
	() => props.draggable,
	(val) => {
		if (val) initTableDraggable();
	},
	{ immediate: true }
);

// Also re-init if data changes (optional, to handle dynamic rows)
watch(
	() => props.data,
	() => {
		if (props.draggable) initTableDraggable();
	}
);

// On mount, set up if draggable
onMounted(() => {
	if (props.draggable) initTableDraggable();
});
// exposure variable
defineExpose({
	pageReset,
});
</script>

<style scoped lang="scss">
.haveFun {
	cursor: pointer;
}

.table-footer-tool {
	display: flex;
	gap: 16px;
}
</style>
