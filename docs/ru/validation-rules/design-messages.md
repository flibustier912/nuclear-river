﻿# Дизайн сообщений об ошибках

На основе агрегатов можно производить расчёт сообщений об ошибках проверок заказов.
Фактически, это ещё одна агрегация поверх агрегатов.  

# Требования к сообщениям об ошибках

Требования от ERM
- быстро найти все сообщения об ошибках для конкретного заказа

Требования от Экспорта:
- быстро найти все сообщения об ошибках для всех заказов в конкретном проекте за конкретный период 

Общие требования
- Машина времени. Должны вернуть информацию об ошибках в заказе на любой известный нам момент времени.    
- Локализация. Для любого проекта нужно уметь получать локализованные сообщения об ошибках на любом языке. Экспорт например захочет сообщения на английском для всех проектов, а UI системам нужно выводит сообщения на языке проекта. 

# Структура агрегатов

![required text](https://immense-sea-86195.herokuapp.com/2gis/nuclear-river/feature/validation-rules/docs/ru/validation-rules/design-messages.puml)

Id агрегата состоит из частей:
- Id и Version заказа
- Начало и конец конкретного периода действия заказа
- Тип сообщения об ошибке.

Остальные поля:
- Id проекта для быстрого поиска по проекту
- Признак IsFailed для быстрого нахождения заказов с ошибками
- MessageParams параметры для подстановки при локализации сообщений об ошибке

Таким образом, агрегат хранит результаты проверки заказа с конкретным Id, отредактированного в версии Version, для каждого возможного периода размещения заказа. Хранятся результаты для всех заказов, как прошедших проверку так и не прошедших. 

Как представленная структура отвечает требованиям?

- Быстрый поиск. Id заказа включён в ключ. Период включён в ключ. Единственный вопрос, что если внешнее API не просить указывать для какого Version вернуть результаты проверки заказов, то нужно будет сортировать заказы по Version, чтобы вернуть результаты по самой свежей проверке.      
> От сортировки по Version можно было бы отказаться введя ещё один уже 4 этап, где хранить результаты только самой свежей проверки, агрегат был бы тот же только без поля Version  

- Машина времени. Id агрегата включает в себя OrderVersion, таким образом хранится информация обо всех заказах во все известных нам версиях 
(что использовать в качестве version вопрос пока открытый)