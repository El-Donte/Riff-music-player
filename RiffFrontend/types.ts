export type Envelope<T = any> = {
  result: T | null;
  errors: ResponseError[] | null;
  timeGenerated: string;
};

export type ResponseError = {
  code: string;
  message: string;
  invalidField?: string | null;
};

export interface Track {
  id: string;
  title: string;
  author: string;
  trackPath: string;
  imagePath: string;
  createdAt: string;
};

export interface User {
  id: string;
  name: string;
  email: string;
  avatarUrl: string;
};