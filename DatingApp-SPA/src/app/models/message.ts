export class Message {
    id: number;
    senderId: number;
    senderKnownAs: string;
    recipientId: number;
    senderPhotoUrl: string;
    recipientKnownAs: string;
    content: string;
    messageSent: Date;
    isRead: boolean;
    readDate: Date;
    isRDeletedecepient: boolean;
    isSenderDeleted: boolean;
}