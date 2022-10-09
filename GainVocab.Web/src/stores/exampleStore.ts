import { defineStore } from "pinia"

interface ExState {
    foo: string
}

export const use = defineStore("store", {
    state: (): ExState => ({
        foo: "foo"
    }),
    actions: {
        updateFoo(foo: string) {
            this.foo = foo
        }
    }
})