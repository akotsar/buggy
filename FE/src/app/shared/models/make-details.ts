import { ModelList } from './model-list';

export interface MakeDetails {
    name: string;
    description: string;
    image: string;
    models: Array<ModelList>;
}
