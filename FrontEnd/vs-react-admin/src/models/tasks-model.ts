export interface Tasks {
    id?: number;
    title: string;
    description: string;
    status: string;
    start_date: string;
    end_date: string;
}

export type tasksPartial = Partial<Tasks>;

export interface Task {
    task: Tasks[];
}