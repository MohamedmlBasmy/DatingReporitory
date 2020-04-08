import { Photo } from './photo';

export interface User {
    id:number;
    description:string;
    username:string;
    gender: string;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: any;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photoUrl: string;
    photos: Photo[];
}
