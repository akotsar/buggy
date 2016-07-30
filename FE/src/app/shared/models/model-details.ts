import { Comment } from './comment';

export class ModelDetails {
    name: string;
    description: string;
    image: string;
    make: string;
    makeId: number;
    makeImage: string;
    votes: number;
    engineVol: number;
    maxSpeed: number;
    comments: Array<Comment>;
    canVote: boolean;
}
